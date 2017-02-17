using System;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace MyFavSpots.Shared
{
	public class InfoView : ContentView
	{
		public Label Name { get; set; }

		public Label Description { get; set; }

		public CachedImage BackgroundImage { get; set; }

		public void SetBindingNames(string name, string desc, string img)
		{
			Name.SetBinding(Label.TextProperty, name);
			Description.SetBinding(Label.TextProperty, desc);
			BackgroundImage.SetBinding(CachedImage.SourceProperty, img);
		}

		public InfoView() : base()
		{
			HeightRequest = 180;

			var layout = new RelativeLayout()
			{
				BackgroundColor = Color.White
			};

			BackgroundImage = new CachedImage()
			{
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 0,
				RetryDelay = 250,
				WidthRequest = layout.Width,
				HeightRequest = 125,
				Aspect = Aspect.AspectFill
			};

			Name = new Label()
			{
				Style = (Style)Application.Current.Resources["infoCellTitleLabel"]
			};

			Description = new Label()
			{
				Style = (Style)Application.Current.Resources["infoCellSubDetailLabel"]
			};

			var box = new BoxView()
			{
				BackgroundColor = Color.FromRgba(0, 0, 0, 100)
			};

			layout.Children.Add(BackgroundImage,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent(p => p.Width),
				Constraint.RelativeToParent(p => p.Height)
			);

			layout.Children.Add(box,
				Constraint.Constant(0),
				Constraint.Constant(120),
				Constraint.RelativeToParent(p => p.Width),
				Constraint.RelativeToParent(p => p.Height - 120)
			);

			layout.Children.Add(Name,
				Constraint.Constant(16),
				Constraint.Constant(133),
				Constraint.RelativeToParent(p => p.Width - 44),
				Constraint.Constant(33)
			);

			layout.Children.Add(Description,
				Constraint.Constant(16),
				Constraint.Constant(133 + 20),
				Constraint.RelativeToParent(p => p.Width - 44),
				Constraint.Constant(33 * 3)
			);

			Content = layout;
		}
	}

	public class BaseInfoCell : ViewCell
	{
		public CachedImage BackgroundImage
		{
			get
			{
				return infoView.BackgroundImage;
			}
		}

		public Label Name
		{
			get
			{
				return infoView.Name;
			}
		}

		public Label Description { 
			get
			{
				return infoView.Description;
			}
		}

		InfoView infoView;

		public BaseInfoCell()
		{
			View = (infoView = new InfoView());
		}

		public void SetBindingNames(string name, string desc, string img)
		{
			infoView.SetBindingNames(name, desc, img);
		}
	}}
