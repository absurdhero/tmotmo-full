The Making of The Making of - Same Not Same
===========================================

Same Not Same lets you experience and puzzle your way through the song of the
same name. The game has the feel and style of a classic adventure game while
letting you use your multi-touching fingers in quirky ways.

The Song
--------

Same Not Same, the song, can be previewed here:

https://itunes.apple.com/us/album/same-not-same-ep/id533347009

Setup
-----

This project requires Unity 3.5 and the basic iOS license to build and run
correctly.  It is possible to run the game with the free Unity editor but you
wouldn't be able to give the game any touch input. You could modify the code to
use mouse input instead!

The first time you open the project, Unity will spend some time importing
assets.  Once it is finished, you should go to File -> Build Settings, select
iOS, and press "Switch Platform." This will let you set the resolution in the
Game window to iPhone Wide (480x320).

Unit Tests and MockUnity
------------------------

There is some unit test coverage of the game located under the Tests directory.
The tests can be opened in MonoDevelop using the tmotmo_tests.sln project file.

The entire Unity environment must be loaded in order to run any code that uses
Unity classes. This makes it difficult to quickly compile and run unit tests
and use normal unit testing tools such as those built in to MonoDevelop.
Additionally, Unity provides no facilities of its own for writing and reporting
test results.

As a result, we developed a set of stub classes that are
interface-compatible with the public Unity library that we can link with
instead of linking with the normal Unity library.

This library serves two purposes:

 - compile our code quickly without any of Unity's dlls or tools.
 - run isolated tests against our game code despite the code depending on Unity classes.

In combination with MockUnity, We use Rhino Mocks
(http://hibernatingrhinos.com/oss/rhino-mocks) to mock unity object behavior
and verify that our Unity-dependent code uses Unity correctly.

License
-------

The source code and build metadata (e.g. project files and scripts) are
licensed under the short and liberal zlib license as found in the LICENSE.CODE
file.

The music is licensed under the Creative Commons CA-NC-SA license (in
LICENSE.MUSIC) which grants non-commercial use.

