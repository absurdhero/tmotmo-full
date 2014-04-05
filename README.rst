The Making of The Making of - Same Not Same
===========================================

Same Not Same lets you experience and puzzle your way through the song of the
same name. The game has the feel and style of a classic adventure game while
letting you use your multi-touching fingers in quirky ways that relate metaphorically 
to the song's lyrics.

The Song
--------

Same Not Same, the song, can be heard here:

https://itunes.apple.com/us/album/same-not-same-ep/id533347009
https://play.google.com/store/music/album?id=Bopkzluqt2ywclj3qzwdb3id5au

Setup
-----

This project requires Unity 3.5 or 4.x and the basic iOS license to build and run
correctly.  It is possible to run the game with the free Unity editor but you
wouldn't be able to give the game any touch input. You could modify the code to
use mouse input instead!

The first time you open the project in Unity, Unity will spend some time importing
assets.  Once it is finished, you should go to File -> Build Settings, select
iOS, and press "Switch Platform." This will let you set the resolution in the
Game window to iPhone Wide (480x320). Currently, other resolutions lead to incorrect 
scaling of the background on non-iOS devices.


Unit Tests and MockUnity
------------------------

There is some unit test coverage of the game located under the Tests directory.
The tests can be opened in MonoDevelop using the tmotmo_tests.sln project file.

The first time you open the Test solution in Xamarin Studio (or MonoDevelop), 
you'll need to install the nuget add-in by following the directions here:
https://github.com/mrward/monodevelop-nuget-addin

Once the nuget add-in is installed, right-click on the solution node in 
the IDE's Solution pad and select "Restore Nuget Packages". This will install
the required versions of NUnit, NMock3, etc so that the project references
resolve correctly. At this point, you should be able to build successfully
and run the tests with 100% passing.

The entire Unity environment must be loaded in order to run any code that uses
Unity classes due to their forced inheritance model. This makes it difficult to 
quickly compile and run unit tests to radidly iterate using normal unit testing tools 
that don't require deploying outside of the development environment (such as those 
built in to MonoDevelop.) Additionally, Unity provides no facilities of its own for 
writing and reporting test results.

As a result, we developed a set of stub classes that are
interface-compatible with the public Unity library that we can link with
instead of linking with the normal Unity library. It is important to remember
that most of these classes don't implement any actual functionality, either because
there are concrete classes whose methods are stubs, or because we have made the concrete
class an interface so it can be easily mocked. For most of the classes,
you will get an exception if code calls a method directly (rather
than using a mock).

This library serves two purposes:

 - compile our code quickly without any of Unity's dlls or tools.
 - run isolated tests against our game code despite the code depending on Unity classes.
 - give us a way to encapsulate our code from the specifics of Unity

In combination with MockUnity, We use NMock3
(http://nmock3.codeplex.com/) to mock unity object behavior
and verify that our Unity-dependent code uses Unity correctly.

License
-------

The source code and build metadata (e.g. project files and scripts) are
licensed under the short and liberal zlib license as found in the LICENSE.CODE
file.

The music is licensed under the Creative Commons CA-NC-SA license (in
LICENSE.MUSIC) which grants non-commercial use.

