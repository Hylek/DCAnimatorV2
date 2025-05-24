# DCAnimator Version 2

A big complete rework of my original 2023 animation tool taking into account shortcoming and experiences from using it.

The point of this tool is to create simple animations quickly that would be too cumbersome or unnecessary to use in Unity's animation graph and animator state machine systems.
## Primary Improvements for Version 2
### Performance
Rather than following the philosophy of Unity's built in animation tools with Animators on GameObjects that need animations and applying them on the update loop of that object we instead centralise this onto the system's own Runtime object. Now all animations are processed in a single Update loop.
### Flexibility
Version 1 was made for a very specific use case - UI menu's opening and closing. This created a very concrete a rigid system that could not easily be repurposed for all kinds of animations. Version 2 allows for animation collections to be used for all sorts of purposes from UI to sprites to movements in cinematics. Animations can also be built entirely programmatically using a Builder pattern inspired by Rust's libraries. Below is a simple fade in example:
```
AnimationBuilder.For(image)
    .AsFade()
    .From(0)
    .To(1)
    .WithDuration(0.5f)
    .WithDelay(1f)
    .Start();
```
Scriptable Object `AnimationCollections` can be used to neatly bundle different kinds of animations together to be used in different contexts. For example, your main menu might have various kinds of fade-in fade-out animations or call-to-actions that are best bundled together. This eliminates the need to repeat manually building animations in code that you might reuse often in different objects.
### Extensible
Version 2 reworks how it handles applying direct changes to game objects as part of it's animations. It now makes use of adapters and builders that are registered with the system to direct it on how it should animate certain component specifics. Extra adapters for specific components can be written and registered with `DCAnimator::AnimationBuilder` allowing you to animate anything you want!
