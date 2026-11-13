# WiFi Tester (WifiOlcer)

**A Windows-based Wi-Fi signal measurement and network analysis tool
built with C# and .NET Framework.**

WiFi Tester (WifiOlcer) is a Windows Forms application that lists nearby
wireless networks and displays information about a selected network,
including its signal strength in dBm. It uses the **Windows WLAN Native
API** to access wireless network information.

## Features

-   List Wi-Fi networks detected by the computer.
-   Select a wireless network by its SSID.
-   Display the selected network's signal strength in dBm.
-   Measure the signal strength of the network currently connected to.
-   Display the SSID and the access point's BSSID (MAC address).
-   Display Wi-Fi channel information.
-   Indicate whether the selected network is the currently connected
    network.
-   Visualize signal-level changes with a ProgressBar.

## Technology

-   **Language:** C#
-   **Framework:** .NET Framework
-   **UI:** Windows Forms
-   **Development environment:** Microsoft Visual Studio
-   **Windows API:** Windows WLAN Native API

## Tested Environment

The application was tested on the following computer and software
configuration:

  Component          Test configuration
  ------------------ ----------------------------------------
  Computer           Lenovo ThinkPad E15 (Type 20RD)
  Processor          Intel Core i5-10210U (10th Generation)
  Wireless adapter   Intel Wi-Fi 6 AX201 160MHz
  Operating system   Windows 11 Pro for Workstations
  Version            26H2
  OS build           26300.9974

### Test observations

During testing, the application:

-   Listed detected Wi-Fi network SSIDs in the ComboBox.
-   Displayed the selected network's BSSID and channel.
-   Indicated whether the selected network matched the currently
    connected network.
-   Displayed RSSI signal strength as a numeric dBm value.
-   Visualized signal-level changes through the ProgressBar.

RSSI values of approximately **-43 dBm to -55 dBm** were observed during
the test. These are observations from the stated test environment, not
guaranteed values. Results may vary depending on the wireless adapter,
driver, access point, distance, and surrounding radio conditions.

## Download

A packaged Windows application is provided in the [`App`](App/) folder.

-   **Beta application package:** [`App/`](App/)

Check the package contents and the detailed documentation for any
runtime or installation requirements before running the application.

## Source Code and Documentation

The Visual Studio project and source files are provided in the
[`Source`](Source/) folder.

-   [English documentation](Source/README_EN.md)
-   [Türkçe dokümantasyon](Source/README_TR.md)

## Project Status

**Beta**

WiFi Tester is under development. Feedback, bug reports, and suggestions
are welcome.

## License

This project is distributed under the **GNU General Public License v3.0
(GPL-3.0)**. See the [`LICENSE`](LICENSE) file in the repository root
for the license terms.

## Related Project

**WifiDroid** is a separate Android application project with a related
goal. It is maintained independently from WiFi Tester; this repository
contains the Windows application only.
