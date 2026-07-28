namespace SAB.Unit
{
    public struct CustomizingData
    {
        public Gender Gender;
        public int Hair;
        public int Eye;
        public int Eyebrow;
        public int Mouth;

        public CustomizingData(Gender gender, int hair, int eye, int eyebrow, int mouth)
        {
            Gender = gender;
            Hair = hair;
            Eye = eye;
            Eyebrow = eyebrow;
            Mouth = mouth;
        }

        public CustomizingData(HumanCustomizingDto dto)
        {
            Gender = dto.Gender;
            Eye = dto.Eye;
            Mouth = dto.Mouth;
            Hair = dto.Hair;
            Eyebrow = dto.Eyebrow;
        }
    }
}
