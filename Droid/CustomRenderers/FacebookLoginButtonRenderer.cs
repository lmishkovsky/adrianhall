using System;
using TaskList.CustomControls;
using TaskList.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Facebook.Login.Widget;

[assembly: ExportRenderer(typeof(FacebookLoginButton), typeof(FacebookLoginButtonRenderer))]
namespace TaskList.Droid.CustomRenderers
{
    /// <summary>
    /// Facebook login button renderer.
    /// </summary>
	public class FacebookLoginButtonRenderer : ViewRenderer<FacebookLoginButton, LoginButton>
	{
		LoginButton facebookLoginButton;

		protected override void OnElementChanged(ElementChangedEventArgs<FacebookLoginButton> e)
		{
			base.OnElementChanged(e);
			if (Control == null || facebookLoginButton == null)
			{
				facebookLoginButton = new LoginButton(Forms.Context);
				SetNativeControl(facebookLoginButton);
			}
		}

	}
}
