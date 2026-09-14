// Copyright Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;

public class BloodDirective : ModuleRules
{
	public BloodDirective(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

		PublicDependencyModuleNames.AddRange(new string[] {
			"Core",
			"CoreUObject",
			"Engine",
			"InputCore",
			"EnhancedInput",
			"AIModule",
			"NavigationSystem",
			"StateTreeModule",
			"GameplayStateTreeModule",
			"Niagara",
			"UMG",
			"Slate"
		});

		PrivateDependencyModuleNames.AddRange(new string[] { });

		PublicIncludePaths.AddRange(new string[] {
			"BloodDirective",
			"BloodDirective/Variant_Strategy",
			"BloodDirective/Variant_Strategy/UI",
			"BloodDirective/Variant_TwinStick",
			"BloodDirective/Variant_TwinStick/AI",
			"BloodDirective/Variant_TwinStick/Gameplay",
			"BloodDirective/Variant_TwinStick/UI"
		});

		// Uncomment if you are using Slate UI
		// PrivateDependencyModuleNames.AddRange(new string[] { "Slate", "SlateCore" });

		// Uncomment if you are using online features
		// PrivateDependencyModuleNames.Add("OnlineSubsystem");

		// To include OnlineSubsystemSteam, add it to the plugins section in your uproject file with the Enabled attribute set to true
	}
}
