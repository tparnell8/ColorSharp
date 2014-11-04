﻿/*
 * The MIT License (MIT)
 * Copyright (c) 2014 Andrés Correa Casablanca
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

/*
 * Contributors:
 *  - Andrés Correa Casablanca <castarco@gmail.com , castarco@litipk.com>
 */


using System;
using System.Collections.Generic;


namespace Litipk.ColorSharp
{
	namespace ColorSpaces
	{
		/**
	 * HP's & Microsoft's 1996 sRGB Color Space.
	 */
		public sealed class SRGB : AConvertibleColor
		{
			#region private properties

			public readonly double R, G, B;

			#endregion


			#region constructors

			/**
		 * This constructor "installs" the conversor methods into the instance
		 */
			private SRGB(AConvertibleColor dataSource=null) : base(dataSource) {
				Conversors.Add (typeof(CIEXYZ), ToCIEXYZ);
			}

			// Constructor
			public SRGB (double R, double G, double B, AConvertibleColor dataSource=null) : this(dataSource)
			{
				this.R = R;
				this.G = G;
				this.B = B;
			}

			#endregion


			#region conversors

			/**
		 * Converts the HP's & Microsoft's 1996 sRGB sample to a CIE 1931 XYZ sample
		 */
			public CIEXYZ ToCIEXYZ (ConversionStrategy strategy=ConversionStrategy.Default)
			{
				// Linear transformation
				double X = R * 0.412424 + G * 0.357579 + B * 0.180464;
				double Y = R * 0.212656 + G * 0.715158 + B * 0.072186;
				double Z = R * 0.019332 + G * 0.119193 + B * 0.950444;

				// Gamma correction
				X = X > 0.04045 ? Math.Pow((X+0.055)/1.055, 2.4) : X/12.92 ;
				Y = Y > 0.04045 ? Math.Pow((Y+0.055)/1.055, 2.4) : Y/12.92 ;
				Z = Z > 0.04045 ? Math.Pow((Z+0.055)/1.055, 2.4) : Z/12.92 ;

				return new CIEXYZ(X, Y, Z, DataSource ?? this);
			}

			#endregion


			#region inherited methods

			public override bool IsInsideColorSpace()
			{
				return (
					0.0 <= R && R <= 1.0 &&
					0.0 <= G && B <= 1.0 &&
					0.0 <= B && B <= 1.0
				);
			}

			public override bool Equals(Object obj)
			{
				SRGB srgbObj = obj as SRGB; 

				if (srgbObj == null || GetHashCode () != obj.GetHashCode ())
					return false;

				return (
					Math.Abs (R - srgbObj.R) <= double.Epsilon &&
					Math.Abs (G - srgbObj.G) <= double.Epsilon &&
					Math.Abs (B - srgbObj.B) <= double.Epsilon
				);
			}
			public override int GetHashCode ()
			{
				int hash = 17;

				hash = hash * 19 + R.GetHashCode ();
				hash = hash * 19 + G.GetHashCode ();
				hash = hash * 19 + B.GetHashCode ();

				return hash;
			}

			#endregion
		}
	}
}


