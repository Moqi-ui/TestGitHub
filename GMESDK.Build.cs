// Copyright 1998-2017 Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;
using System;
using System.IO;

public class GMESDK : ModuleRules
{
#if WITH_FORWARDED_MODULE_RULES_CTOR
	public GMESDK(ReadOnlyTargetRules Target) : base(Target)
#else
    public GMESDK(TargetInfo Target)
#endif
	{
        string GMELibPath = string.Empty;

		//PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;
		
		PublicIncludePaths.AddRange(
			new string[] {
				"GMESDK/Public",
				"GMESDK/Public/GMESDK",
				// ... add public include paths required here ...
			}
			);
				
		
		PrivateIncludePaths.AddRange(
			new string[] {
				"GMESDK/Private",
				// ... add other private include paths required here ...
			}
			);
			
		
		PublicDependencyModuleNames.AddRange(
			new string[]
			{
				"Core",
				"Projects",
				"GMESDKLibrary"
				// ... add other public dependencies that you statically link with here ...
			}
			);
			
		
		PrivateDependencyModuleNames.AddRange(
			new string[]
			{
				// ... add private dependencies that you statically link with here ...	
			}
			);
		
		
		DynamicallyLoadedModuleNames.AddRange(
			new string[]
			{
				// ... add any modules that your module loads dynamically here ...
			}
			);


        string PluginPath = Utils.MakePathRelativeTo(ModuleDirectory, Target.RelativeEnginePath);
        string GMEThirdPartyDir = Path.GetFullPath(Path.Combine(ModuleDirectory, "../ThirdParty/GMESDKLibrary"));

        System.Console.WriteLine("-------------- PluginPath = " + PluginPath);

        if (Target.Platform == UnrealTargetPlatform.Android)
        {
            PrivateDependencyModuleNames.AddRange(new string[] { "Launch" });

            string aplPath = Path.Combine(PluginPath, "GMESDK_APL.xml");
            System.Console.WriteLine("-------------- AplPath = " + aplPath);
            AdditionalPropertiesForReceipt.Add(new ReceiptProperty("AndroidPlugin", aplPath));
        }
        else if (Target.Platform == UnrealTargetPlatform.IOS )
        {
            PrivateIncludePaths.Add("GMESDK/Private/iOS");
            PublicIncludePaths.AddRange(new string[] {"Runtime/ApplicationCore/Public/Apple", "Runtime/ApplicationCore/Public/IOS"});

            PrivateDependencyModuleNames.AddRange(
                new string[]
                {
                    "ApplicationCore"
                    // ... add private dependencies that you statically link with here ...
                }
                );
        }

        if (Target.Platform == UnrealTargetPlatform.Win32)
        {
            string OSVersion = (Target.Platform == UnrealTargetPlatform.Win32) ? "x86" : "x64";
            string libDir = OSVersion;
            GMELibPath = Path.Combine(GMEThirdPartyDir, libDir);
            PublicLibraryPaths.Add(GMELibPath);
            Console.WriteLine("GMELibPath:" + GMELibPath);

            PublicAdditionalLibraries.AddRange(
                new string[] {
                    "gmesdk.lib",
                }
                );

            string binariesDir = Path.Combine(ModuleDirectory, "../../Binaries", Target.Platform.ToString());
            if (!Directory.Exists(binariesDir))
                Directory.CreateDirectory(binariesDir);

            string[] dllNames = new string[] {
                    "gmesdk.dll",
                };

            foreach (string dllName in dllNames)
            {
                RuntimeDependencies.Add(new RuntimeDependency(Path.Combine(GMELibPath, dllName)));
            }

            try
            {
                foreach (string dllName in dllNames)
                {
                    System.Console.WriteLine("src dll=" + dllName + " dst dir=" + binariesDir);
                    File.Copy(Path.Combine(GMELibPath, dllName), Path.Combine(binariesDir, dllName), true);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine("copy dll exception,maybe have exists,err=", e.ToString());
            }
        }
        if (Target.Platform == UnrealTargetPlatform.Win64)
        {
            string OSVersion = (Target.Platform == UnrealTargetPlatform.Win32) ? "x86" : "x64";
            string libDir = OSVersion;
            GMELibPath = Path.Combine(GMEThirdPartyDir, libDir);
            PublicLibraryPaths.Add(GMELibPath);
            Console.WriteLine("GMELibPath:" + GMELibPath);

            PublicAdditionalLibraries.AddRange(
                new string[] {
                    "gmesdk.lib",
                }
                );

            string binariesDir = Path.Combine(ModuleDirectory, "../../Binaries", Target.Platform.ToString());
            if (!Directory.Exists(binariesDir))
                Directory.CreateDirectory(binariesDir);

            string[] dllNames = new string[] {
                    "gmesdk.dll",
                };

            foreach (string dllName in dllNames)
            {
                RuntimeDependencies.Add(new RuntimeDependency(Path.Combine(GMELibPath, dllName)));
            }

            try
            {
                foreach (string dllName in dllNames)
                {
                    System.Console.WriteLine("src dll=" + dllName + " dst dir=" + binariesDir);
                    File.Copy(Path.Combine(GMELibPath, dllName), Path.Combine(binariesDir, dllName), true);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine("copy dll exception,maybe have exists,err=", e.ToString());
            }
        }
        else if (Target.Platform == UnrealTargetPlatform.Android)
        {
            string libDir = ("Android/GME/libs");
            string ArchDir = "armeabi-v7a";
            GMELibPath = Path.Combine(Path.Combine(GMEThirdPartyDir, libDir), ArchDir);
            System.Console.WriteLine("--------------Android GME Lib path = " + GMELibPath);
            PublicLibraryPaths.Add(GMELibPath);

            ArchDir = "arm64-v8a";
            GMELibPath = Path.Combine(Path.Combine(GMEThirdPartyDir, libDir), ArchDir);
            System.Console.WriteLine("--------------Android GME Lib path = " + GMELibPath);
            PublicLibraryPaths.Add(GMELibPath);

            ArchDir = "x86";
            GMELibPath = Path.Combine(Path.Combine(GMEThirdPartyDir, libDir), ArchDir);
            System.Console.WriteLine("--------------Android GME Lib path = " + GMELibPath);
            PublicLibraryPaths.Add(GMELibPath);

            ArchDir = "x86_64";
            GMELibPath = Path.Combine(Path.Combine(GMEThirdPartyDir, libDir), ArchDir);
            System.Console.WriteLine("--------------Android GME Lib path = " + GMELibPath);
            PublicLibraryPaths.Add(GMELibPath);

            PublicAdditionalLibraries.Add("traeimp");
            PublicAdditionalLibraries.Add("gmesdk");
        }
        else if (Target.Platform == UnrealTargetPlatform.Mac)
        {
            GMELibPath = GMEThirdPartyDir;
            string strLib = GMELibPath + "/Mac/libGMESDK.a";
            PublicAdditionalLibraries.Add(strLib);
            System.Console.WriteLine("---framework path:" + strLib);

            string strSystemLibPath = "/Applications/Xcode.app/Contents/Developer/Platforms/MacOSX.platform/Developer/SDKs/MacOSX.sdk/usr/lib/";
            PublicAdditionalLibraries.AddRange(
                new string[] {
                    strSystemLibPath + "libc++.tbd",
                    "z",
                    "iconv",
                    "resolv",
                }
                );

            PublicFrameworks.AddRange(
                new string[] {
                    "Foundation",
                    "AVFoundation",
                    "CoreTelephony",
                    "Security",
                    "SystemConfiguration",
                    "AudioToolbox",
                    "CoreMedia",
                    "CoreAudio",
                    "CoreVideo",
                    "OpenAL",
                }
                );
        }
        else if (Target.Platform == UnrealTargetPlatform.IOS)
        {
            GMELibPath = GMEThirdPartyDir;
            string strLib = GMELibPath + "/iOS/libGMESDK.a";
            PublicAdditionalLibraries.Add(strLib);
            System.Console.WriteLine("---framework path:" + strLib);

            PublicAdditionalLibraries.AddRange(
                new string[] {
                    "c++",
                    "c++.1",
                    "z",
                    "iconv",
                    "resolv",
                }
                );

            PublicFrameworks.AddRange(
                new string[] {
                    "Foundation",
                    "AVFoundation",
                    "CoreTelephony",
                    "Security",
                    "SystemConfiguration",
                    "AudioToolbox",
                    "CoreMedia",
                    "CoreAudio",
                    "CoreVideo",
                    "OpenAL",
                }
                );
        }
		else if (Target.Platform == UnrealTargetPlatform.Switch)
        {
            GMELibPath = Path.Combine(GMEThirdPartyDir, "NX64");
            PublicLibraryPaths.Add(GMELibPath);
            PublicAdditionalLibraries.AddRange(
                new string[] {
                    "GME",
                    "UDT",
                    "TRAE",
                }
                );
            Console.WriteLine("Switch GMELibPath:" + GMELibPath);
		}
		else if (Target.Platform == UnrealTargetPlatform.PS4)
        {
            GMELibPath = Path.Combine(GMEThirdPartyDir, "PS4");
            PublicLibraryPaths.Add(GMELibPath);
            PublicAdditionalLibraries.AddRange(
                new string[] {
                    "GME",
                    "UDT",
                    "TRAE",
                }
                );
            Console.WriteLine("PS4 GMELibPath:" + GMELibPath);
        }
		else if (Target.Platform == UnrealTargetPlatform.XboxOne)
        {
            GMELibPath = Path.Combine(GMEThirdPartyDir, "XboxOne");
            PublicLibraryPaths.Add(GMELibPath);
            Console.WriteLine("GMELibPath:" + GMELibPath);

            PublicAdditionalLibraries.AddRange(
                new string[] {
                    "gmesdk.lib"
                }
                );
            Console.WriteLine("XboxOne GMELibPath:" + GMELibPath);

            string binariesDir = Path.Combine(ModuleDirectory, "../../Binaries", Target.Platform.ToString());
            if (!Directory.Exists(binariesDir))
                Directory.CreateDirectory(binariesDir);

            string[] dllNames = new string[] {
                    "gmesdk.dll",
                };
        }
    }
}

