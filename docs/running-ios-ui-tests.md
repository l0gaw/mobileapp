### How to run UI tests for iOS

#### Before we begin

While running tests on iOS is much simpler than on Android, there are some caveats to consider. The main one is that when running the UI tests on a simulator, you have to have `Hardware > Keyboard > Connect Hardware Keyboard` unchecked, otherwise the test runner will not be able to input anything. There's also a general glitchiness to UI tests on iOS which we've handled by restarting the emulator before each test.

##### To run on Visual Studio:
- Set build target to `iOS | Debug | Any` or `iOS | Debug iPhone | Your device` to run on a simulator or your device respectively
- On the right sidebar > Unit Tests > iOS > Toggl.iOS.Tests.UI
- On Test Apps select the device where you want to run the tests on.
- If you don't see the tests there yet, double click iOS > Toggl.iOS.Tests.UI; fix whatever bugs might be there (the build might be broken) > click again > You'll eventually see all the tests from Shared.Toggl.Tests.UI;
- On Toggl.Tests.UI you can run all the tests or only the ones you want.
- ✨

##### To run on Rider:
- Set build target to `Debug | iPhoneSimulator`
- Set whatever device you want to run the tests on in the Run Configurations
- Just go to Unit Tests -> Toggl.Tests.UI and run them as normal tests
