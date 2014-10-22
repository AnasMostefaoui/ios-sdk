﻿using System;
using System.Collections.Generic;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;
using MonoTouch.CoreGraphics;

using TelerikUI;

namespace Examples
{
	public class CustomAnimationAreaChart: ExampleViewController
	{
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			TKChart chart = new TKChart (this.ExampleBounds);
			chart.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			chart.Delegate = new ChartDelegate ();
			chart.AllowAnimations = true;
			this.View.AddSubview (chart);

			Random r = new Random ();
			List<TKChartDataPoint> list = new List<TKChartDataPoint> ();
			for (int i = 0; i < 10; i++) {
				list.Add (new TKChartDataPoint(new NSNumber(i), new NSNumber(r.Next() % 100)));
			}

			TKChartAreaSeries areaSeries = new TKChartAreaSeries (list.ToArray ());
			chart.AddSeries (areaSeries);
		}

		class ChartDelegate: TKChartDelegate
		{
			public override CAAnimation AnimationForSeries (TKChart chart, TKChartSeries series, TKChartSeriesRenderState state, RectangleF rect)
			{
				double duration = 0.5;
				List<CAAnimation> animations = new List<CAAnimation> ();

				for (int i = 0; i<state.Points.Count; i++) {
					string keyPath = string.Format ("seriesRenderStates.{0}.points.{1}.y", series.Index, i);
					TKChartVisualPoint point = (TKChartVisualPoint)state.Points.ObjectAtIndex((uint)i);
					double oldY = rect.Height;	
					double half = oldY + (point.Y - oldY)/2.0;
					CAKeyFrameAnimation a = (CAKeyFrameAnimation)CAKeyFrameAnimation.GetFromKeyPath(keyPath);
					a.KeyTimes = new NSNumber[] { new NSNumber (0), new NSNumber (0), new NSNumber (1) };
					a.Values = new NSObject[] { new NSNumber (oldY), new NSNumber (half), new NSNumber (point.Y) };
					a.Duration = duration;
					a.TimingFunction = CAMediaTimingFunction.FromName (CAMediaTimingFunction.EaseOut);
					animations.Add(a);
				}
	
				CAAnimationGroup group = new CAAnimationGroup ();

				group.Duration = duration;
				group.Animations = animations.ToArray();

				return group;
			}
		}
	}
}

