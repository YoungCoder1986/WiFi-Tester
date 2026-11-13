using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace WifiOlcer
{
    public partial class FRMWIFI : Form
    {
        public FRMWIFI()
        {
            InitializeComponent();
        }

        internal static class WifiNative
        {


            public static bool GetSelectedNetworkConnectionStatus(string selectedItem)
            {
                LastError = "";

                if (string.IsNullOrWhiteSpace(selectedItem))
                {
                    LastError = "Ağ seçilmedi.";
                    return false;
                }

                int openParen = selectedItem.LastIndexOf('(');
                int closeParen = selectedItem.LastIndexOf(')');

                if (openParen < 0 || closeParen <= openParen)
                {
                    LastError = "BSSID okunamadı.";
                    return false;
                }

                string selectedBssid = selectedItem
                    .Substring(openParen + 1, closeParen - openParen - 1)
                    .Trim();

                IntPtr clientHandle = IntPtr.Zero;
                IntPtr interfaceList = IntPtr.Zero;

                try
                {
                    int result = WlanOpenHandle(
                        2, IntPtr.Zero, out _, out clientHandle);

                    if (result != 0)
                    {
                        LastError = "WlanOpenHandle hatası: " + result;
                        return false;
                    }

                    result = WlanEnumInterfaces(
                        clientHandle, IntPtr.Zero, out interfaceList);

                    if (result != 0)
                    {
                        LastError = "WlanEnumInterfaces hatası: " + result;
                        return false;
                    }

                    int count = Marshal.ReadInt32(interfaceList, 0);

                    int infoSize = Marshal.SizeOf<WLAN_INTERFACE_INFO>();

                    IntPtr firstInfo = IntPtr.Add(interfaceList, 8);

                    for (int i = 0; i < count; i++)
                    {
                        WLAN_INTERFACE_INFO info =
                            Marshal.PtrToStructure<WLAN_INTERFACE_INFO>(
                                IntPtr.Add(firstInfo, i * infoSize));

                        uint dataSize;
                        IntPtr data;
                        int opcodeValueType;

                        result = WlanQueryInterface(
                            clientHandle,
                            ref info.InterfaceGuid,
                            7,
                            IntPtr.Zero,
                            out dataSize,
                            out data,
                            out opcodeValueType);

                        if (result != 0 || data == IntPtr.Zero)
                            continue;

                        try
                        {
                            WLAN_CONNECTION_ATTRIBUTES connection =
                                Marshal.PtrToStructure<WLAN_CONNECTION_ATTRIBUTES>(
                                    data);

                            // 1 = WLAN_INTERFACE_STATE_CONNECTED
                            if (connection.isState != 1)
                                continue;

                            string connectedBssid = BitConverter
                                .ToString(
                                    connection.wlanAssociationAttributes.dot11Bssid)
                                .Replace('-', ':');

                            if (string.Equals(
                                selectedBssid,
                                connectedBssid,
                                StringComparison.OrdinalIgnoreCase))
                            {
                                LastError = "Seçilen ağa bağlı.";
                                return true;
                            }
                        }
                        finally
                        {
                            WlanFreeMemory(data);
                        }
                    }

                    LastError = "Seçilen ağa bağlı değil.";
                    return false;
                }
                finally
                {
                    if (interfaceList != IntPtr.Zero)
                        WlanFreeMemory(interfaceList);

                    if (clientHandle != IntPtr.Zero)
                        WlanCloseHandle(clientHandle, IntPtr.Zero);
                }
            }





            private static int FrequencyToChannel(uint frequencyKHz)
            {
                double frequencyMHz = frequencyKHz / 1000.0;

                // 2.4 GHz
                if (Math.Abs(frequencyMHz - 2484) < 1)
                    return 14;

                if (frequencyMHz >= 2412 && frequencyMHz <= 2472)
                    return (int)Math.Round((frequencyMHz - 2407) / 5);

                // 5 GHz
                if (frequencyMHz >= 5000 && frequencyMHz < 5925)
                    return (int)Math.Round((frequencyMHz - 5000) / 5);

                // 6 GHz
                if (frequencyMHz >= 5955 && frequencyMHz <= 7115)
                    return (int)Math.Round((frequencyMHz - 5950) / 5);

                return 0;
            }












            public static int? GetSelectedNetworkChannel(string selectedItem)
            {
                LastError = "";

                if (string.IsNullOrWhiteSpace(selectedItem))
                {
                    LastError = "Ağ seçilmedi.";
                    return null;
                }

                // ComboBox biçimi: SSID (AA:BB:CC:DD:EE:FF)
                int openParen = selectedItem.LastIndexOf('(');
                int closeParen = selectedItem.LastIndexOf(')');

                if (openParen < 0 || closeParen <= openParen)
                {
                    LastError = "BSSID okunamadı.";
                    return null;
                }

                string selectedBssid = selectedItem
                    .Substring(openParen + 1, closeParen - openParen - 1)
                    .Trim();

                IntPtr clientHandle = IntPtr.Zero;
                IntPtr interfaceList = IntPtr.Zero;

                int result = WlanOpenHandle(
                    2, IntPtr.Zero, out _, out clientHandle);

                if (result != 0)
                {
                    LastError = "WlanOpenHandle hata kodu: " + result;
                    return null;
                }

                try
                {
                    result = WlanEnumInterfaces(
                        clientHandle, IntPtr.Zero, out interfaceList);

                    if (result != 0)
                    {
                        LastError = "WlanEnumInterfaces hata kodu: " + result;
                        return null;
                    }

                    int count = Marshal.ReadInt32(interfaceList, 0);
                    int infoSize = Marshal.SizeOf<WLAN_INTERFACE_INFO>();
                    IntPtr firstInfo = IntPtr.Add(interfaceList, 8);

                    for (int i = 0; i < count; i++)
                    {
                        WLAN_INTERFACE_INFO info =
                            Marshal.PtrToStructure<WLAN_INTERFACE_INFO>(
                                IntPtr.Add(firstInfo, i * infoSize));

                        IntPtr bssList = IntPtr.Zero;

                        result = WlanGetNetworkBssList(
                            clientHandle,
                            ref info.InterfaceGuid,
                            IntPtr.Zero,
                            3,
                            false,
                            IntPtr.Zero,
                            out bssList);

                        if (result != 0)
                        {
                            LastError = "WlanGetNetworkBssList hata kodu: " + result;
                            continue;
                        }

                        try
                        {
                            if (bssList == IntPtr.Zero)
                                continue;

                            // WLAN_BSS_LIST:
                            // dwTotalSize is at offset 0, dwNumberOfItems at offset 4.
                            int bssTotalSize = Marshal.ReadInt32(bssList, 0);
                            int bssCount = Marshal.ReadInt32(bssList, 4);

                            // Native counts and sizes must be validated before pointer arithmetic.
                            if (bssTotalSize < 8 || bssCount < 0)
                            {
                                LastError = "WlanGetNetworkBssList geçersiz liste döndürdü.";
                                continue;
                            }

                            //long entryOffset = 8L + (long)j * entrySize;
                            //if (entryOffset > bssTotalSize - entrySize)
                            //{
                            //    lastBssError = "WlanGetNetworkBssList kayıt boyutu geçersiz.";
                            //    break;
                            //}

                            int entrySize = Marshal.SizeOf<WLAN_BSS_ENTRY>();
                            IntPtr firstEntry = IntPtr.Add(bssList, 8);

                            for (int j = 0; j < bssCount; j++)
                            {
                                long entryOffset = 8L + (long)j * entrySize;
                                if (entryOffset > bssTotalSize - entrySize)
                                {
                                    LastError = "WlanGetNetworkBssList kayıt boyutu geçersiz.";
                                    break;
                                }

                                IntPtr entryPtr = IntPtr.Add(
                                    firstEntry, checked(j * entrySize));

                                WLAN_BSS_ENTRY bssEntry =
                                    Marshal.PtrToStructure<WLAN_BSS_ENTRY>(
                                        entryPtr);

                                if (bssEntry.dot11Bssid == null ||
                                    bssEntry.dot11Bssid.Length != 6)
                                    continue;

                                string bssid = BitConverter
                                    .ToString(bssEntry.dot11Bssid)
                                    .Replace('-', ':');

                                if (string.Equals(
                                    bssid,
                                    selectedBssid,
                                    StringComparison.OrdinalIgnoreCase))
                                {
                                    int channel =
                                        FrequencyToChannel(bssEntry.ulChCenterFrequency);

                                    if (channel > 0)
                                    {
                                        LastError = "Kanal başarıyla okundu.";
                                        return channel;
                                    }

                                    LastError = "Frekans tanınamadı.";
                                    return null;
                                }
                            }
                        }
                        finally
                        {
                            if (bssList != IntPtr.Zero)
                                WlanFreeMemory(bssList);
                        }
                    }

                    if (string.IsNullOrEmpty(LastError))
                        LastError = "Seçilen BSSID için kanal bulunamadı.";

                    return null;
                }
                finally
                {
                    if (interfaceList != IntPtr.Zero)
                        WlanFreeMemory(interfaceList);

                    if (clientHandle != IntPtr.Zero)
                        WlanCloseHandle(clientHandle, IntPtr.Zero);
                }


            }








            public static string LastError { get; private set; } = "";

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            private struct WLAN_INTERFACE_INFO
            {
                public Guid InterfaceGuid;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
                public string strInterfaceDescription;

                public int isState;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct DOT11_SSID
            {
                public uint uSSIDLength;

                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32,
                    ArraySubType = UnmanagedType.U1)]
                public byte[] ucSSID;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct WLAN_RATE_SET
            {
                public uint uRateSetLength;

                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 126)]
                public ushort[] usRateSet;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct WLAN_BSS_ENTRY
            {
                public DOT11_SSID dot11Ssid;
                public uint uPhyId;

                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6,
                    ArraySubType = UnmanagedType.U1)]
                public byte[] dot11Bssid;

                public int dot11BssType;
                public int dot11BssPhyType;
                public int lRssi;
                public uint uLinkQuality;

                [MarshalAs(UnmanagedType.U1)]
                public bool bInRegDomain;

                public ushort usBeaconPeriod;
                public ulong ullTimestamp;
                public ulong ullHostTimestamp;
                public ushort usCapabilityInformation;
                public uint ulChCenterFrequency;
                public WLAN_RATE_SET wlanRateSet;
                public uint ulIeOffset;
                public uint ulIeSize;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct WLAN_ASSOCIATION_ATTRIBUTES
            {
                public DOT11_SSID dot11Ssid;
                public int dot11BssType;

                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6,
                    ArraySubType = UnmanagedType.U1)]
                public byte[] dot11Bssid;

                public int dot11PhyType;
                public uint uPhyIndex;
                public uint wlanSignalQuality;
                public uint ulRxRate;
                public uint ulTxRate;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct WLAN_SECURITY_ATTRIBUTES
            {
                public int bSecurityEnabled;
                public int bOneXEnabled;
                public int dot11AuthAlgorithm;
                public int dot11CipherAlgorithm;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            private struct WLAN_CONNECTION_ATTRIBUTES
            {
                public int isState;
                public int wlanConnectionMode;

                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
                public string strProfileName;

                public WLAN_ASSOCIATION_ATTRIBUTES wlanAssociationAttributes;
                public WLAN_SECURITY_ATTRIBUTES wlanSecurityAttributes;
            }







            [DllImport("wlanapi.dll")]
            private static extern int WlanOpenHandle(
                uint clientVersion,
                IntPtr reserved,
                out uint negotiatedVersion,
                out IntPtr clientHandle);



            [DllImport("wlanapi.dll")]
            private static extern int WlanQueryInterface(
            IntPtr clientHandle,
            ref Guid interfaceGuid,
            int opCode,
            IntPtr reserved,
            out uint dataSize,
            out IntPtr data,
            out int opcodeValueType);


            [DllImport("wlanapi.dll")]
            private static extern int WlanEnumInterfaces(
                IntPtr clientHandle,
                IntPtr reserved,
                out IntPtr interfaceList);

            [DllImport("wlanapi.dll")]
            private static extern int WlanGetNetworkBssList(
                IntPtr clientHandle,
                ref Guid interfaceGuid,
                IntPtr dot11Ssid,
                int bssType,
                [MarshalAs(UnmanagedType.Bool)] bool securityEnabled,
                IntPtr reserved,
                out IntPtr bssList);

            [DllImport("wlanapi.dll")]
            private static extern void WlanFreeMemory(IntPtr memory);

            [DllImport("wlanapi.dll")]
            private static extern int WlanCloseHandle(
                IntPtr clientHandle,
                IntPtr reserved);

            public static int? GetSelectedNetworkRssi(string selectedItem)
            {
                LastError = "";

                if (string.IsNullOrWhiteSpace(selectedItem))
                {
                    LastError = "ComboBox'ta ağ seçilmemiş.";
                    return null;
                }

                // ComboBox biçimi: SSID (AA:BB:CC:DD:EE:FF)
                int openParen = selectedItem.LastIndexOf('(');
                int closeParen = selectedItem.LastIndexOf(')');

                if (openParen < 0 || closeParen <= openParen)
                {
                    LastError = "Seçilen ağın BSSID biçimi okunamadı.";
                    return null;
                }

                string selectedBssid = selectedItem
                    .Substring(openParen + 1, closeParen - openParen - 1)
                    .Trim();

                if (selectedBssid.Length != 17)
                {
                    LastError = "BSSID adresi geçersiz: " + selectedBssid;
                    return null;
                }

                IntPtr clientHandle = IntPtr.Zero;
                IntPtr interfaceList = IntPtr.Zero;

                int result = WlanOpenHandle(
                    2, IntPtr.Zero, out _, out clientHandle);

                if (result != 0)
                {
                    LastError = "WlanOpenHandle hata kodu: " + result;
                    return null;
                }

                try
                {
                    result = WlanEnumInterfaces(
                        clientHandle, IntPtr.Zero, out interfaceList);

                    if (result != 0)
                    {
                        LastError = "WlanEnumInterfaces hata kodu: " + result;
                        return null;
                    }

                    int interfaceCount = Marshal.ReadInt32(interfaceList, 0);
                    int interfaceSize = Marshal.SizeOf<WLAN_INTERFACE_INFO>();
                    IntPtr firstInterface = IntPtr.Add(interfaceList, 8);

                    if (interfaceCount == 0)
                    {
                        LastError = "Windows Wi-Fi adaptörü bulamadı.";
                        return null;
                    }

                    int totalBssCount = 0;
                    string lastBssError = "";

                    for (int i = 0; i < interfaceCount; i++)
                    {
                        IntPtr interfacePtr = IntPtr.Add(
                            firstInterface, i * interfaceSize);

                        WLAN_INTERFACE_INFO info =
                            Marshal.PtrToStructure<WLAN_INTERFACE_INFO>(
                                interfacePtr);

                        IntPtr bssList = IntPtr.Zero;

                        result = WlanGetNetworkBssList(
                            clientHandle,
                            ref info.InterfaceGuid,
                            IntPtr.Zero,
                            3,
                            false,
                            IntPtr.Zero,
                            out bssList);

                        if (result != 0)
                        {
                            lastBssError =
                                "WlanGetNetworkBssList hata kodu: " + result;
                            continue;
                        }

                        try
                        {
                            if (bssList == IntPtr.Zero)
                                continue;

                            // WLAN_BSS_LIST:
                            // dwTotalSize is at offset 0, dwNumberOfItems at offset 4.
                            int bssTotalSize = Marshal.ReadInt32(bssList, 0);
                            int bssCount = Marshal.ReadInt32(bssList, 4);

                            // Native counts and sizes must be validated before pointer arithmetic.
                            if (bssTotalSize < 8 || bssCount < 0)
                            {
                                lastBssError = "WlanGetNetworkBssList geçersiz liste döndürdü.";
                                continue;
                            }

                            totalBssCount += bssCount;

                            int entrySize = Marshal.SizeOf<WLAN_BSS_ENTRY>();
                            IntPtr firstEntry = IntPtr.Add(bssList, 8);

                            for (int j = 0; j < bssCount; j++)
                            {
                                long entryOffset = 8L + (long)j * entrySize;
                                if (entryOffset > bssTotalSize - entrySize)
                                {
                                    lastBssError = "WlanGetNetworkBssList kayıt boyutu geçersiz.";
                                    break;
                                }

                                IntPtr entryPtr = IntPtr.Add(
                                    firstEntry, checked(j * entrySize));

                                WLAN_BSS_ENTRY bssEntry =
                                    Marshal.PtrToStructure<WLAN_BSS_ENTRY>(
                                        entryPtr);

                                if (bssEntry.dot11Bssid == null ||
                                    bssEntry.dot11Bssid.Length != 6)
                                    continue;

                                string bssid = BitConverter
                                    .ToString(bssEntry.dot11Bssid)
                                    .Replace('-', ':');

                                if (string.Equals(
                                    bssid,
                                    selectedBssid,
                                    StringComparison.OrdinalIgnoreCase))
                                {
                                    LastError = "RSSI başarıyla okundu.";
                                    return bssEntry.lRssi;
                                }
                            }
                        }
                        finally
                        {
                            if (bssList != IntPtr.Zero)
                                WlanFreeMemory(bssList);
                        }
                    }

                    if (!string.IsNullOrEmpty(lastBssError))
                    {
                        LastError = lastBssError;
                    }
                    else
                    {
                        LastError =
                            "BSSID bulunamadı: " + selectedBssid +
                            ". Arayüz sayısı: " + interfaceCount +
                            ", bulunan BSS kaydı: " + totalBssCount;
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    LastError = "RSSI okuma hatası: " + ex.Message;
                    return null;
                }
                finally
                {
                    if (interfaceList != IntPtr.Zero)
                        WlanFreeMemory(interfaceList);

                    if (clientHandle != IntPtr.Zero)
                        WlanCloseHandle(clientHandle, IntPtr.Zero);
                }
            }
        }





        private void MeasureSelectedNetwork()
        {
            if (CBSSID.SelectedItem == null)
                return;

            int? signalDbm = WifiNative.GetSelectedNetworkRssi(
                CBSSID.SelectedItem.ToString());

            if (signalDbm.HasValue)
            {
                UpdateSignalDisplay(signalDbm.Value);
            }
            else
            {
                LBLSEVIYE.Text = "Ölçüm yok";
                PBSINYAL.Value = 0;

                // Tanılama mesajını Debug Output penceresine yazar.
                System.Diagnostics.Debug.WriteLine(
                    WifiNative.LastError);
            }
        }









        private void ScanWifiNetworks()
        {
            // 1. Tarama başlamadan önce seçili ağı sakla
            string selectedNetwork = CBSSID.SelectedItem?.ToString();

            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "netsh.exe";
                    process.StartInfo.Arguments = "wlan show networks mode=bssid";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;

                    process.Start();

                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    // 2. ComboBox listesini güncelle
                    CBSSID.BeginUpdate();

                    try
                    {
                        CBSSID.Items.Clear();

                        string currentSSID = "";

                        foreach (string line in output.Split(
                            new[] { "\r\n", "\n" },
                            StringSplitOptions.None))
                        {
                            Match ssidMatch = Regex.Match(
                                line, @"^\s*SSID\s+\d+\s*:\s*(.*)$");

                            if (ssidMatch.Success)
                            {
                                currentSSID = ssidMatch.Groups[1].Value.Trim();
                                continue;
                            }

                            Match bssidMatch = Regex.Match(
                                line,
                                @"^\s*BSSID\s+\d+\s*:\s*" +
                                @"([0-9A-Fa-f]{2}(?::[0-9A-Fa-f]{2}){5})");

                            if (bssidMatch.Success)
                            {
                                string macAddress =
                                    bssidMatch.Groups[1].Value.ToUpperInvariant();

                                string networkName =
                                    string.IsNullOrWhiteSpace(currentSSID)
                                    ? "(Gizli ağ)"
                                    : currentSSID;

                                string item = $"{networkName} ({macAddress})";

                                if (!CBSSID.Items.Contains(item))
                                {
                                    CBSSID.Items.Add(item);
                                }
                            }
                        }

                        // 3. Tarama bittikten sonra seçimi geri yükle
                        if (!string.IsNullOrEmpty(selectedNetwork) &&
                            CBSSID.Items.Contains(selectedNetwork))
                        {
                            CBSSID.SelectedItem = selectedNetwork;
                        }
                        else if (CBSSID.Items.Count > 0)
                        {
                            CBSSID.SelectedIndex = 0;
                        }
                    }
                    finally
                    {
                        CBSSID.EndUpdate();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Wi-Fi ağları taranamadı: " + ex.Message,
                    "WiFi Tester",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private void StartRefreshTimer()
        {
            TMRREFRESH.Interval = 10000; // 10 saniye
            TMRREFRESH.Start();
        }



        private void FRMWIFI_Load(object sender, EventArgs e)
        {
            ScanWifiNetworks();
            StartRefreshTimer();

            if (CBSSID.Items.Count > 0)
            {
                CBSSID.Text = CBSSID.Items[0].ToString();
            }
            TMRREFRESH.Start();

        }

        private void TMRREFRESH_Tick(object sender, EventArgs e)
        {
            ScanWifiNetworks();
            MeasureSelectedNetwork();

        }
    
    

        private void UpdateSignalDisplay(int signalDbm)
        {
            int signalPercent = (int)Math.Round(
         (signalDbm + 100) * 100.0 / 70.0);

            signalPercent = Math.Max(0, Math.Min(100, signalPercent));

            PBSINYAL.Minimum = 0;
            PBSINYAL.Maximum = 100;
            PBSINYAL.Value = signalPercent;

            LBLSEVIYE.Text = $"{signalDbm} dBm";
        }

        private void CBSSID_SelectedIndexChanged(object sender, EventArgs e)
        {
            MeasureSelectedNetwork();
            LBLSSIDNAME.Text = CBSSID.SelectedItem?.ToString() ?? "Seçili ağ yok";

            if (CBSSID.SelectedItem == null)
                return;

            string selectedNetwork = CBSSID.SelectedItem.ToString();

            // SSID ve MAC adresini göster
            LBLSSIDNAME.Text = selectedNetwork;

            // Sinyal ölçümünü yap
            MeasureSelectedNetwork();

            // Kanal bilgisini oku
            int? channel =
                WifiNative.GetSelectedNetworkChannel(selectedNetwork);

            // Kanal bilgisini Label üzerinde göster
            if (channel.HasValue && channel.Value > 0)
            {
                LBLCHANNEL.Text = channel.Value.ToString();
            }
            else
            {
                LBLCHANNEL.Text = "Bilinmiyor";
            }

            bool isConnected =
    WifiNative.GetSelectedNetworkConnectionStatus(selectedNetwork);

            LBLSTATUS.Text = isConnected ? "Bağlı" : "Bağlı değil";


        }
    }


    }
