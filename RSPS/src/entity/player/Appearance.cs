namespace RSPS.src.entity.player
{
    public struct Appearance
    {

        public int Gender { get; private set; }
		public int Chest { get; private set; }
		public int Arms { get; private set; }
		public int Legs { get; private set; }
		public int Head { get; private set; }
		public int Hands { get; private set; }
		public int Feet { get; private set; }
		public int Beard { get; private set; }
		public int HairColor { get; private set; }
		public int TorsoColor { get; private set; }
		public int LegColor { get; private set; }
		public int FeetColor { get; private set; }
		public int SkinColor { get; private set; }

		public Appearance()
		{
			Gender = 0;
			Head = 0;
			Chest = 18;
			Arms = 26;
			Hands = 33;
			Legs = 36;
			Feet = 42;
			Beard = 10;
			HairColor = 7;
			TorsoColor = 8;
			LegColor = 9;
			FeetColor = 5;
			SkinColor = 0;
		}

		public int[] GetLook()
		{
			return new[] {
				Gender,
				HairColor,
				TorsoColor,
				LegColor,
				FeetColor,
				SkinColor,
				Head,
				Chest,
				Arms,
				Hands,
				Legs,
				Feet,
				Beard
			};
		}

		public void SetLook(int[] look)
        {
			if (look.Length != 13)
            {
				throw new ArgumentException("Look array must be length of 13");
            }

			Gender = look[0];
			Head = look[6];
			Chest = look[7];
			Arms = look[8];
			Hands = look[9];
			Legs = look[10];
			Feet = look[11];
			Beard = look[12];
			HairColor = look[1];
			TorsoColor = look[2];
			LegColor = look[3];
			FeetColor = look[4];
			SkinColor = look[5];
        }

	}
}
