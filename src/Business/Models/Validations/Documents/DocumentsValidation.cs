namespace Business.Models.Validations.Documents
{
    public class ZipCodeValidation
    {
        public const int ZipCodeLength = 11;

        public static bool Validate(string zipCode)
        {
            var zipCodeNumbers = Utils.OnlyNumbers(zipCode);
            if (!IsValidLength(zipCodeNumbers)) return false;
            return !HasRepeatedDigits(zipCodeNumbers) && HasValidDigits(zipCodeNumbers);
        }

        private static bool IsValidLength(string value)
        {
            return value.Length == ZipCodeLength;
        }

        private static bool HasRepeatedDigits(string value)
        {
            string[] invalidNumbers =
            {
                "00000000000",
                "11111111111",
                "22222222222",
                "33333333333",
                "44444444444",
                "55555555555",
                "66666666666",
                "77777777777",
                "88888888888",
                "99999999999"
            };
            return invalidNumbers.Contains(value);
        }

        private static bool HasValidDigits(string value)
        {
            var number = value.Substring(0, ZipCodeLength - 2);
            var verifyingDigit = new VerifyingDigit(number)
                .WithMultipliersUpTo(2, 11)
                .Replacing("0", 10, 11);
            var firstDigit = verifyingDigit.CalculateDigit();
            verifyingDigit.AddDigit(firstDigit);
            var secondDigit = verifyingDigit.CalculateDigit();
            return string.Concat(firstDigit, secondDigit) == value.Substring(ZipCodeLength - 2, 2);
        }
    }

    public class CnpjValidation
    {
        public const int CnpjLength = 14;

        public static bool Validate(string cpnj)
        {
            var cnpjNumbers = Utils.OnlyNumbers(cpnj);
            if (!HasValidLength(cnpjNumbers)) return false;
            return !HasRepeatedDigits(cnpjNumbers) && HasValidDigits(cnpjNumbers);
        }

        private static bool HasValidLength(string value)
        {
            return value.Length == CnpjLength;
        }

        private static bool HasRepeatedDigits(string value)
        {
            string[] invalidNumbers =
            {
                "00000000000000",
                "11111111111111",
                "22222222222222",
                "33333333333333",
                "44444444444444",
                "55555555555555",
                "66666666666666",
                "77777777777777",
                "88888888888888",
                "99999999999999"
            };
            return invalidNumbers.Contains(value);
        }

        private static bool HasValidDigits(string value)
        {
            var number = value.Substring(0, CnpjLength - 2);
            var verifyingDigit = new VerifyingDigit(number)
                .WithMultipliersUpTo(2, 9)
                .Replacing("0", 10, 11);
            var firstDigit = verifyingDigit.CalculateDigit();
            verifyingDigit.AddDigit(firstDigit);
            var secondDigit = verifyingDigit.CalculateDigit();
            return string.Concat(firstDigit, secondDigit) == value.Substring(CnpjLength - 2, 2);
        }
    }

    public class VerifyingDigit
    {
        private const int Module = 11;
        private string _number;
        private readonly List<int> _multipliers = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9 };
        private readonly IDictionary<int, string> _replacements = new Dictionary<int, string>();
        private bool _moduleComplement = true;

        public VerifyingDigit(string numero)
        {
            _number = numero;
        }

        public VerifyingDigit WithMultipliersUpTo(int firstMultiplier, int lastMultiplier)
        {
            _multipliers.Clear();
            for (var i = firstMultiplier; i <= lastMultiplier; i++)
                _multipliers.Add(i);
            return this;
        }

        public VerifyingDigit Replacing(string substitute, params int[] digits)
        {
            foreach (var i in digits)
                _replacements[i] = substitute;
            return this;
        }

        public void AddDigit(string digit)
        {
            _number = string.Concat(_number, digit);
        }

        public string CalculateDigit()
        {
            return !(_number.Length > 0) ? "" : GetDigitSum();
        }

        private string GetDigitSum()
        {
            var sum = 0;
            for (int i = _number.Length - 1, m = 0; i >= 0; i--)
            {
                var produto = (int)char.GetNumericValue(_number[i]) * _multipliers[m];
                sum += produto;

                if (++m >= _multipliers.Count) m = 0;
            }
            var mod = (sum % Module);
            var result = _moduleComplement ? Module - mod : mod;
            return _replacements.ContainsKey(result) ? _replacements[result] : result.ToString();
        }
    }

    public class Utils
    {
        public static string OnlyNumbers(string value)
        {
            var onlyNumber = "";
            foreach (var s in value)
            {
                if (char.IsDigit(s)) onlyNumber += s;
            }
            return onlyNumber.Trim();
        }
    }
}