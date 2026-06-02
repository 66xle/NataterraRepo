Version 1.14
Fixed the bug that stopped the DismantledEvent from being called.
Added a test script to the demo scene to show the use of the DismantledEvent.
Fixed a bug where if you have a target that is different to the object that the artificer component is attached to and that target has scaling then the assmebly and dismantle animations could show wrong size elements.

Version 1.13
Fixed wrong message in Inspector when part building an object it would show Dismantling instead of Assembling
Fixed a bug when using the SetBuiltLevel method, the wrong bounds value was being used so the parts might be clipped from the view.
Fixed the Part Build not working correctly in demo scene
BuildParts works correctly, was not setting the buildmode correctly.

Version 1.12
Fixed a bug when you added a prefab to a scene that was not already in the scene or had been used and then called SetBuiltLevel on it, the mesh was not setup.

Version 1.11
SetBuildLevel now clamps the alpha value passed to it to 0 - 1, and writes a warning to the console.
Changed the check for object being active in hierarchy to active self in the build process so building objects in prefabs and not in scenes works correctly now.

Version 1.10
Fixed a possible error if you had inactive renderers in your objects hierarchy.

Version 1.09
Fixed a bug when using copies of Artificer objects and the names of any of the objects were changed, would cause an exception. Rewritten to use unique ids instead of names.
Note. You will need to rebuild any Artificer objects that have built data. You can use the Tools/Artificer/Build All In Scene to do that quickly.

Version 1.08
Fixed a bug if Material sorting was selected and you also have remapped materials, the sort order would be wrong.
Added more help videos

Version 1.07
Added a URP Double Sided shadergraph and material, useful for using in material remap to handle inside of objects.
Added a HDRP Double Sided shadergraph and material, useful for using in material remap to handle inside of objects.
Added a BuiltIn Double Sided shadergraph and material, useful for using in material remap to handle inside of objects.
Enabled the click build system. Allows you to build an object one piece at a time with a clickable object, shows the next piece to be built.
Added a demo scene to show the click build system.
BuildData() method replaced with RebuildData() use this method at runtime if you need to rebuild the build data at all.
Clear button added by the BuildData field in the inspector, this allows you to clear the data and so not have it included in the build and the data will be built automatically at runtime

Version 1.06
Added ability to instantly set any level of build on an object.
Updated demo scene to include a Set Build button and a slider to test the instant build feature.
Added a SetBuiltLevel method to allow you to instantly set the build state of the object with a normalized value between 0 and 1.
Added a ContinueBuild method to allow you to have the Build continue after using the SetBuiltLevel method
Added a PauseBuild method to pause any current build in progress.
Added a IsPaused method to check if the build is paused.

Version 1.05
Updated materials in the demo scene to use Universal Lit shaders, so they will now convert using the Unity Material Wizard to other pipelines.
If you are importing into a Unity project not using URP then the DOFControl script will give an error, to fix open the file and comment out the define at the top.
Added a new method to allow you render unbuilt and placing elements in a set material, perfect for showing what is about to be built.
Updated demo scene with toggles for showing unbuilt and placing elements.

Version 1.04
Added the Build Parts system so you can easily control building the objects in steps from script instead of having the whole object build in one go.
More video help added
Added IsBuilding method so you can easily check if an object is being built
Added IsDismantling method so you can easily check if an object is being dismantled.
Added StartPartBuild method so you can control the building of an object from script.
Added BuildParts method so you can control the building of an object from script.
Updated demo scene to include a BuildParts example.

Version 1.03
Added some help video options to the Build Section, more coming. Click the icon on the right for a video on the value.
Added an option to Artificer preferences to disable the Video Help Icons.
If a help video is available it will also be an option in the right click on the value help menu that opens up.

Version 1.02
Fixed a bug when using a scaled Unity spline as a build from object and the Move Along option was set, spline path was not calculated correctly.
Fixed exception if you had a non spline object as a build from location but had Move Along option set.

Version 1.01
Added a Bounds Mode which can reduce CPU usage by 50%. Calc mode will recalculate the render bounds for each Mesh element which is totally accurate but has a CPU cost, Value mode allows you to set a general bounds size which will be used for all element saving the recalc. New option is in the Extra Options section of the Inspector.
Fixed Help not working in the Extra Options part of the Inspector.
Fixed a missing material in the demo scene.

Version 1.00
Initial Release

Online Docs: https://tubbycrumbles.gitbook.io/Artificer
Email: chris@west-racing.com

For Support please visit our Discord https://discord.gg/yuYbbDB and message me with your Invoice details and you will get access to the Artificer Support channel.