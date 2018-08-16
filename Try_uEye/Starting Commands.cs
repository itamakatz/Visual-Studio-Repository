// 1.Initialize the camera: 
cam.Init();

// 2.Allocate an default image memory: 
cam.Memory.Allocate(out s32MemId);
// You can also allocate an image memory by using: 
cam.Memory.Allocate();

// 3.Capture a live image with 
cam.Acquisition.Capture(s32Wait); 
// or a single image with 
cam.Acquisition.Freeze(s32Wait);

// With 
s32Wait = uEye.Defines.DeviceParameter.Wait 
// the image acquisition waits until an image is captured and returns afterwards. With 
s32Wait = uEye.Defines.DeviceParameter.DontWait 
// the image acquisition returns immediately.

// 4.Display the image on the screen: 
cam.Display.Render(uEye.Defines.DisplayRenderMode mode)
// mode selects different rendering modes, e.g. 
mode = uEye.Defines.DisplayRenderMode.FitToWindow.