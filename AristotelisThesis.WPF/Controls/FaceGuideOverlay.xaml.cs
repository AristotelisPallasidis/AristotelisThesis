using System.Windows.Controls;

namespace AristotelisThesis.WPF.Controls
{
    /// <summary>
    /// A transparent, non-interactive oval guide overlaid on the camera feed so the user
    /// positions their face at a consistent distance for capture/recognition.
    /// </summary>
    public partial class FaceGuideOverlay : UserControl
    {
        public FaceGuideOverlay()
        {
            InitializeComponent();
        }
    }
}
