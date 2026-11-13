using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace WifiOlcer
{


    namespace WifiOlcer
    {

        using System;
        using System.Runtime.InteropServices;

        namespace WifiOlcer
        {
            public  class WifiNative
            {
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

                    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 126,
                        ArraySubType = UnmanagedType.U2)]
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

                [DllImport("wlanapi.dll")]
                private static extern int WlanOpenHandle(
                    uint clientVersion, IntPtr reserved,
                    out uint negotiatedVersion, out IntPtr clientHandle);

                [DllImport("wlanapi.dll")]
                private static extern int WlanEnumInterfaces(
                    IntPtr clientHandle, IntPtr reserved,
                    out IntPtr interfaceList);

                [DllImport("wlanapi.dll")]
                private static extern int WlanGetNetworkBssList(
                    IntPtr clientHandle, ref Guid interfaceGuid,
                    IntPtr dot11Ssid, int bssType,
                    bool securityEnabled, IntPtr reserved,
                    out IntPtr bssList);

                [DllImport("wlanapi.dll")]
                private static extern void WlanFreeMemory(IntPtr memory);

                [DllImport("wlanapi.dll")]
                private static extern int WlanCloseHandle(
                    IntPtr clientHandle, IntPtr reserved);

                // BSSID metnini alır ve eşleşen erişim noktasının RSSI değerini döndürür.
                public static int? GetSelectedNetworkRssi(string selectedItem)
                {
                    if (string.IsNullOrWhiteSpace(selectedItem))
                        return null;

                    int openResult = WlanOpenHandle(
                        2, IntPtr.Zero, out _, out IntPtr clientHandle);

                    if (openResult != 0)
                        return null;

                    try
                    {
                        int enumResult = WlanEnumInterfaces(
                            clientHandle, IntPtr.Zero, out IntPtr interfaceList);

                        if (enumResult != 0)
                            return null;

                        try
                        {
                            if (interfaceList == IntPtr.Zero)
                                return null;

                            // WLAN_INTERFACE_INFO_LIST.dwNumberOfItems is offset 0.
                            int count = Marshal.ReadInt32(interfaceList, 0);
                            int infoSize = Marshal.SizeOf<WLAN_INTERFACE_INFO>();
                            IntPtr firstInfo = IntPtr.Add(interfaceList, 8);

                            // ComboBox metninin sonundaki MAC/BSSID adresini ayıkla.
                            int openParen = selectedItem.LastIndexOf('(');
                            int closeParen = selectedItem.LastIndexOf(')');

                            if (openParen < 0 || closeParen <= openParen)
                                return null;

                            string selectedBssid = selectedItem
                                .Substring(openParen + 1, closeParen - openParen - 1)
                                .Trim();

                            for (int i = 0; i < count; i++)
                            {
                                IntPtr infoPtr = IntPtr.Add(
                                    firstInfo, i * infoSize);

                                WLAN_INTERFACE_INFO info =
                                    Marshal.PtrToStructure<WLAN_INTERFACE_INFO>(
                                        infoPtr);

                                int bssResult = WlanGetNetworkBssList(
                                    clientHandle,
                                    ref info.InterfaceGuid,
                                    IntPtr.Zero,
                                    3, false, IntPtr.Zero,
                                    out IntPtr bssList);

                                if (bssResult != 0)
                                    continue;

                                try
                                {
                                    if (bssList == IntPtr.Zero)
                                        continue;

                                    // WLAN_BSS_LIST:
                                    // dwTotalSize is at offset 0, dwNumberOfItems at offset 4.
                                    int bssTotalSize = Marshal.ReadInt32(bssList, 0);
                                    int bssCount = Marshal.ReadInt32(bssList, 4);

                                    if (bssTotalSize < 8 || bssCount < 0)
                                        continue;

                                    int entrySize = Marshal.SizeOf<WLAN_BSS_ENTRY>();
                                    IntPtr firstEntry = IntPtr.Add(bssList, 8);

                                    for (int j = 0; j < bssCount; j++)
                                    {
                                        long entryOffset = 8L + (long)j * entrySize;
                                        if (entryOffset > bssTotalSize - entrySize)
                                            break;

                                        WLAN_BSS_ENTRY bssEntry =
                                            Marshal.PtrToStructure<WLAN_BSS_ENTRY>(
                                                IntPtr.Add(firstEntry, checked(j * entrySize)));

                                        if (bssEntry.dot11Bssid == null ||
                                            bssEntry.dot11Bssid.Length != 6)
                                            continue;

                                        string bssid = BitConverter
                                            .ToString(bssEntry.dot11Bssid)
                                            .Replace('-', ':');

                                        if (string.Equals(
                                            bssid, selectedBssid,
                                            StringComparison.OrdinalIgnoreCase))
                                        {
                                            return bssEntry.lRssi;
                                        }
                                    }
                                }
                                finally
                                {
                                    WlanFreeMemory(bssList);
                                }
                            }

                            return null;
                        }
                        finally
                        {
                            WlanFreeMemory(interfaceList);
                        }
                    }
                    finally
                    {
                        WlanCloseHandle(clientHandle, IntPtr.Zero);
                    }
                }
            }
        }


    }
}