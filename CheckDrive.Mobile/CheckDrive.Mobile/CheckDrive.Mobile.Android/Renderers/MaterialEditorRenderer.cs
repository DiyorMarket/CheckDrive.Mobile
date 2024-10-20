using Android.Content;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using CheckDrive.Mobile.Droid.Renderers;

[assembly: ExportRenderer(typeof(Editor), typeof(MaterialEditorRenderer))]
namespace CheckDrive.Mobile.Droid.Renderers
{
    public class MaterialEditorRenderer : EditorRenderer
    {
        public MaterialEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                // Remove the underline
                Control.Background = null;
            }
        }
    }
}