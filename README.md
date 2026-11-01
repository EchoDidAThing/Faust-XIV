#Echo's TODO
- add A way to prevent all changes until validated for a new patch.
- add dev override for that stop so we can actually do fixes.
- More graceful configfile handling to allow for it to handle changes in data.
- When Saving Data, Only save edited values.
- Fix config loading to skip unknown sets.
- Add some sort of config verification.
- -Add copypath buttons for stuff like vfx.
- Add more features - housing light removal, housing vfx endpoint adding, scene lights for various things so we can leverage more than point lights, action and status vfx and animation custom paths, adding, animation attaching beyond the player to allow for mounts to leverage stuff like tp animations.
- more UI stuff.
- clean up code some more so we can have a testing version.
- maybe interface adjustments. 

#taco's to-do:
- limit input values to prevent the log from going haywire
- refactoring broke sheet calls when reenabling the plugin somehow. need to fix it
- figure out how to streamline some of the fields
- more sheets to break stuff
- find out how to circumvent handling of the packed bools


`FieldOffset(0x??)` is the actual address being updated. Lumina sheets are just used for pulling base data. so, technically, if the column doesn't exist in the source data, a dummy field can be supplied, although that does make things slightly obtuse.


# ActionTimelineReplacement, a.k.a. ATR

Special credit to Loskh for the original development, and to DethiCra for completely rebuilding the plugin, as well as creating the interface.


Use `/faust` to access the config window quicker.

## Q&A

### Can these changes be detected by the server?
As far as we are aware they should not be. There is an effort here to ensure everything is client side and impacts what the server sees as little as possible.
