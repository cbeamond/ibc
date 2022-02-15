namespace Ironbelly
{
	public static partial class Constant
	{
		public static class Layer
		{
			public static class Id
			{
				public const int Shootable = 6;
			}

			public static class Mask
			{
				public const int Shootable = 1 << Id.Shootable;
			}

			public static class Name
			{
				public const string Shootable = nameof(Shootable);
			}
		}
	}
}