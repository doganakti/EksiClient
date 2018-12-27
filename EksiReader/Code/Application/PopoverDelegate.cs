using System;
using UIKit;

namespace EksiReader
{
    /// <summary>
    /// Popover delegate.
    /// </summary>
    public class PopoverDelegate : UIPopoverPresentationControllerDelegate
    {
        static PopoverDelegate _instance;
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static PopoverDelegate Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PopoverDelegate();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Gets the adaptive presentation style.
        /// </summary>
        /// <returns>The adaptive presentation style.</returns>
        /// <param name="forPresentationController">For presentation controller.</param>
        public override UIModalPresentationStyle GetAdaptivePresentationStyle(UIPresentationController forPresentationController)
        {
            return UIModalPresentationStyle.None;
        }

        /// <summary>
        /// Configures the presentation.
        /// </summary>
        /// <returns>The presentation.</returns>
        /// <param name="viewController">View controller.</param>
        public static UIPopoverPresentationController ConfigurePresentation(UIViewController viewController)
        {
            viewController.ModalPresentationStyle = UIModalPresentationStyle.Popover;
            var presentationController = viewController.PopoverPresentationController;
            presentationController.Delegate = Instance;
            return presentationController;
        }
    }
}
