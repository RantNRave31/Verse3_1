using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Diagnostics;

using GKYU.PresentationLogicLibrary.Settings;
using GKYU.PresentationLogicLibrary.ViewModels;
using GKYU.PresentationLogicLibrary.Models;
using GKYU.BusinessLogicLibrary.Bitmaps;

namespace GKYU.PresentationLogicLibrary.ViewModels
{
    public class EquipmentViewModel
        : FileViewModel
    {
        public int Port { get; set; }

        private MANUFACTURER _manufacturer;
        public MANUFACTURER Manufacturer
        {
            get
            {
                return _manufacturer;
            }
            set
            {
                if (value == _manufacturer)
                    return;
                _manufacturer = value;
                OnPropertyChanged("Manufacturer");
            }
        }

        public Queue<Thread> WorkerThreads;
        public EquipmentViewModel(string displayName, FileModel fileModel)
            : base(displayName, fileModel)
        {
            WorkerThreads = new Queue<Thread>();
        }
        public void ResetPinPad()
        {
            Process process = new Process();
            process.StartInfo.FileName = "SET_MTX.EXE";
            process.StartInfo.Arguments = string.Format("{0} XRST", Port);
            process.StartInfo.UseShellExecute = true;
            process.Start();
            process.WaitForExit();
        }
        public void LoadPinPadOS()
        {
            Process process = new Process();
            process.StartInfo.FileName = "SET_MTX.EXE";
            process.StartInfo.Arguments = string.Format("{0} XDLD {1} P 2", Port, FileName);
            process.StartInfo.UseShellExecute = true;
            process.Start();
            process.WaitForExit();
        }
        public void LoadPinPadFormAgent()
        {
            Process process = new Process();
            process.StartInfo.FileName = "SET_MTX.EXE";
            process.StartInfo.Arguments = string.Format("{0} XDLD {1} P 2", Port, FileName);
            process.StartInfo.UseShellExecute = true;
            process.Start();
            process.WaitForExit();
        }
        public void LoadPinPadCTLS()
        {
            Process process = new Process();
            process.StartInfo.FileName = "SET_MTX.EXE";
            process.StartInfo.Arguments = string.Format("{0} XDLD {1} P 2", Port, FileName);
            process.StartInfo.UseShellExecute = true;
            process.Start();
            process.WaitForExit();
        }
        public void AddPinPadParameter(string key, string code, string value)
        {
            Configuration.Setting configurationSetting = null;
            if (!PropertyMap.ContainsKey(key))
            {
                configurationSetting = new Configuration.Setting() { Name = key, Value = value };
                PropertyMap.AddOnUI(new KeyValuePair<string, Configuration.Setting>(key, configurationSetting));
                Properties.AddOnUI(configurationSetting);
                Thread.Sleep(30);
            }
            else
            {
                configurationSetting = PropertyMap[key];
                configurationSetting.Value = value;
            }
        }
        public void LoadPinPadKeys()
        {
            Process process = new Process();
            process.StartInfo.FileName = "SET_MTX.EXE";
            process.StartInfo.Arguments = string.Format("{0} XDLD {1} P 2", Port, FileName);
            process.StartInfo.UseShellExecute = true;
            process.Start();
            process.WaitForExit();
        }
        protected string[] ParseSetting(string input)
        {
            return input.Split('=');
        }
        public string[] ParseVersion(string input)
        {
            return input.Split(',');
        }
        protected void PullPinPadParameter(string key)
        {
            Process process = new Process();
            string program = "SET_MTX.EXE";
            string arguments = string.Empty;
            switch (key)
            {
                case "Version":
                    arguments = string.Format("{0} XVER", Port);
                    break;
                case "Baud Rate":
                    arguments = string.Format("{0} Q bd", Port);
                    break;
                case "Retalix Key":
                    arguments = string.Format("{0} Q E2EE_KSN", Port);
                    break;
                default:
                    throw new Exception("Invalid Property Key");
            }
            process.StartInfo.FileName = program;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.OutputDataReceived += (sender, args) =>
            {
                string[] setting = null;
                string newKey = string.Empty;
                string code = string.Empty;
                string value = string.Empty;
                if (args.Data != null)
                {
                    switch (key)
                    {
                        case "Version":
                            string packageData = string.Empty;
                            foreach (string data in ParseVersion(args.Data))
                            {
                                setting = ParseSetting(data);
                                if (setting[0] == "XVER")
                                    continue;// skip XVER
                                code = setting[0];
                                value = setting[1];
                                //"XVER,APP=5.0.11-20180302,XPIAPP=0052.00y-20180427,XPIKERNEL=000270000,OS=MX200001/RFS30251000/,PACKAGES=alsa-lib:1.0.0; busybox:1.2.6.2501; captouch-flash:1.0.3.2501; cdgnetcfg:1.0.22.2509; cdgnetctrl:1.0.23.2509; dropbear:2012.55.3; ecr-lib:1.0.2; fancypants:1.0.20.2501; fonts-flash:1.0.10; hantrolib:1.0.6; i2c-tools:1.0.0; kmods:1.1.3; libgpl:1.0.0; libgraphic:1.0.3; libvp:1.0.8; libvpcfg:1.0.5; logapi:1.0.10.2504; logo_screen:1.0.0; mxlegacy:1.4.25.2501; netutils:1.0.1; openssl:1.0.6; powermgt:1.0.18.2502; secins:1.12.11.2506; securemods:1.0.11.2503; security:1.3.12.2507; security-flash:4.2.6; skeletonfs:1.1.19.2518; svcstklibs:1.0.13; vfi-utils:1.2.16.2502; vfilib:1.1.23.2501; vfimods:1.1.28.2509; vfinetctrl:1.0.4; z_vats_unlock_rfs:1.0.0; z_vats_unlock_secrfs:1.0.0; zz_ctls_l2-security:1.0.45.00; zz_ctlshw-update:1.0.4; cgisvcstack:1.0.2; ecr-app:1.0.1; ecr-tools:1.0.1; mediaserver:1.0.6; svcmgrconfs:1.0.9; svcmgrstack:1.0.15; vfiservices:1.1.18.2513; vfiservices-dram:1.0.21.2505; vfiservices-flsh:1.0.12; vos-syslog-flash:1.0.0.2503; sred-version:5.1.20s; vcl:5.1.20s; sysmode:1.5.25.2504; sysmode-themes:1.0.25.2504; zz_ctls_l2:4-1.16.10A4; powermgtctl:1.0.18.2502; powermgtctl-flash:1.0.16.2502; vhq_svc:1.0.7; vhq_sys:1.4.9.1401; vhq_sys-certs:1.2.6; printd:1.0.13; ProdWIC:4014; VSS:1.0; WIC_INI:4014; WIC_Modules:4014; bin:1.1; configini:1.0; configusr1:1.8; faflash:2.2; fafrms:1.2; frmAgent:5.0.11; guimgr:3.27; lib_aci:2.5; lib_bintable:2.0; lib_ctls:2.30p2; lib_dbg:2.13; lib_emvtbl:2.0; lib_ini:1.8; lib_odu:1.9-1.10; lib_rfcr:5.26; lib_rsa:4.2; lib_tcpip:2.0.19; lib_vcl:2.4; lib_vhq:2.5; lib_vsd:2.6; lib_xpi:5200y2; libs_other:1.3; pubkey:1.0; ssl:1.0; xpifrms:171221; xpivss:1.0; zbin:1.0; zcfg:16.09.01; zcfgini:1.0; zflash:1.4; zfrm:18.05.25; zpci3:16.08.10; zrfidon:16.09.01; zue2ee:on; zusb:16.08.10; zxpifrm:17.10.24,
                                ////GUIMGR=3.27,UNITID=281-360-380/12000000,MODEL=MX925,RFIDFW=VOS-CTLS-4-1.16.10A4,CLOCK=20211213165358,SECUREPACKETS=AES128"
                                switch (code)
                                {
                                    case "APP": newKey = "Application"; AddPinPadParameter(newKey, code, value); break;
                                    case "XPIAPP": newKey = "XPI Application"; AddPinPadParameter(newKey, code, value); break;
                                    case "XPIKERNEL": newKey = "XPI Kernel"; AddPinPadParameter(newKey, code, value); break;
                                    case "OS": newKey = "OS"; AddPinPadParameter(newKey, code, value); break;
                                    case "PACKAGES":
                                        packageData = data;
                                        break;

                                    case "GUIMGR": newKey = "GUI Manager"; AddPinPadParameter(newKey, code, value); break;
                                    case "UNITID": newKey = "Unit ID"; AddPinPadParameter(newKey, code, value); break;
                                    case "MODEL": newKey = "Model"; AddPinPadParameter(newKey, code, value); break;
                                    case "RFIDFW": newKey = "RFID Frequency"; AddPinPadParameter(newKey, code, value); break;
                                    case "CLOCK": newKey = "Date/Time"; AddPinPadParameter(newKey, code, value); break;
                                    case "SECUREPACKETS": newKey = "Secure Packets"; AddPinPadParameter(newKey, code, value); break;
                                    default: newKey = key; AddPinPadParameter(newKey, code, value); break;
                                }

                            }
                            if (packageData.Length > 0)
                                foreach (string package in packageData.Split(';'))
                                {
                                    setting = package.Split(':');
                                    AddPinPadParameter("Package:" + setting[0], setting[0], setting[1]);
                                }
                            break;
                        default:
                            setting = ParseSetting(args.Data);
                            code = setting[0];
                            value = setting[1];
                            AddPinPadParameter(key, code, value);
                            break;
                    }
                }
            };
            process.BeginOutputReadLine();
            process.WaitForExit();

        }
        public void PullPinPadParameters()
        {
            PullPinPadParameter("Version");
            PullPinPadParameter("Baud Rate");
            PullPinPadParameter("Retalix Key");
        }
    }
}
