using System.Windows.Controls;

namespace AristotelisThesis.WPF.Controls
{
    /// <summary>
    /// A transparent, non-interactive guide overlaid on the camera feed so the user positions
    /// their open right palm at a consistent distance for capture/recognition. The silhouette is
    /// drawn mirror-style: a right hand with the thumb on the right of the frame.
    /// </summary>
    public partial class PalmGuideOverlay : UserControl
    {
        public PalmGuideOverlay()
        {
            InitializeComponent();
        }
    }
}
