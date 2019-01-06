// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace EksiReader
{
    [Register ("TopicsHeaderVC")]
    partial class TopicsHeaderVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl SegmentedControl { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (SegmentedControl != null) {
                SegmentedControl.Dispose ();
                SegmentedControl = null;
            }
        }
    }
}