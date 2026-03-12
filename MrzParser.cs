using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace QR1000Reader
{
    /// <summary>
    /// MRZ 解析结果
    /// </summary>
    public class MrzResult
    {
        public string DocumentNumber { get; set; } = "";
        public string Surname { get; set; } = "";
        public string GivenNames { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Nationality { get; set; } = "";
        public string DateOfBirth { get; set; } = "";
        public string Sex { get; set; } = "";
        public string DateOfExpiry { get; set; } = "";
        public string IssuingState { get; set; } = "";
        public bool IsValid { get; set; } = false;
        public string MrzText { get; set; } = "";
    }

    /// <summary>
    /// MRZ 解析器
    /// </summary>
    public static class MrzParser
    {
        /// <summary>
        /// 从识别数据中解析 MRZ 信息
        /// </summary>
        public static MrzResult ParseFromFields(Dictionary<string, string> fields)
        {
            var result = new MrzResult();

            // 1. 优先使用 OCR MRZ
            if (fields.ContainsKey("OCR MRZ") && !string.IsNullOrEmpty(fields["OCR MRZ"]))
            {
                result.MrzText = fields["OCR MRZ"].Trim();
                result = ParseMrzText(result.MrzText);
                if (result.IsValid)
                {
                    return result;
                }
            }

            // 2. 合并 MRZ1 + MRZ2 + MRZ3
            string mrz1 = fields.ContainsKey("MRZ1") ? fields["MRZ1"].Trim() : "";
            string mrz2 = fields.ContainsKey("MRZ2") ? fields["MRZ2"].Trim() : "";
            string mrz3 = fields.ContainsKey("MRZ3") ? fields["MRZ3"].Trim() : "";

            if (!string.IsNullOrEmpty(mrz1))
            {
                result.MrzText = mrz1;
                if (!string.IsNullOrEmpty(mrz2))
                {
                    result.MrzText += " " + mrz2;
                }
                if (!string.IsNullOrEmpty(mrz3))
                {
                    result.MrzText += " " + mrz3;
                }

                result = ParseMrzText(result.MrzText);
            }

            return result;
        }

        /// <summary>
        /// 解析 MRZ 文本
        /// </summary>
        private static MrzResult ParseMrzText(string mrzText)
        {
            var result = new MrzResult();
            
            // 清理 MRZ 文本，移除空格和特殊字符，但保留字母数字和<
            string cleanMrz = Regex.Replace(mrzText.ToUpper(), @"[^A-Z0-9<]", "");

            if (string.IsNullOrEmpty(cleanMrz) || cleanMrz.Length < 30)
            {
                return result;
            }

            // 检测证件类型
            string docType = cleanMrz.Substring(0, 2);

            // TD1 格式（身份证，3 行 x 30 字符）
            if (docType.Contains("I") || docType.Contains("A") || docType.Contains("C") || 
                docType.StartsWith("H") || docType.StartsWith("M"))
            {
                ParseTd1Format(cleanMrz, result);
            }
            // TD3 格式（护照，2 行 x 44 字符）
            else if (docType.StartsWith("P") || docType.StartsWith("V"))
            {
                ParseTd3Format(cleanMrz, result);
            }
            else
            {
                // 尝试通用解析
                ParseGenericFormat(cleanMrz, result);
            }

            return result;
        }

        /// <summary>
        /// 解析 TD1 格式（3 行 x 30 字符）
        /// </summary>
        private static void ParseTd1Format(string mrz, MrzResult result)
        {
            try
            {
                // 提取第 3 行（姓名行）- 从位置 60 开始
                if (mrz.Length >= 90)
                {
                    string nameLine = mrz.Substring(60, 30);
                    ParseNameFromMrz(nameLine, result);
                }
                else if (mrz.Length >= 30)
                {
                    // 如果只有 1 行，尝试从第 3 个位置开始找姓名
                    int namePos = mrz.IndexOf("<<");
                    if (namePos > 0)
                    {
                        ParseNameFromMrz(mrz.Substring(namePos), result);
                    }
                }

                // 提取证件号码 - 从第 1 行位置 6 开始
                if (mrz.Length >= 15)
                {
                    string docNum = mrz.Substring(6, 9);
                    result.DocumentNumber = docNum.TrimEnd('<').TrimEnd('0').Replace("<", "");
                }

                // 提取国籍
                if (mrz.Length >= 5)
                {
                    result.Nationality = mrz.Substring(3, 3);
                }

                // 提取出生日期（YYMMDD）
                if (mrz.Length >= 18)
                {
                    string dob = mrz.Substring(12, 6);
                    result.DateOfBirth = ParseDate(dob);
                }

                // 提取性别
                if (mrz.Length >= 19)
                {
                    result.Sex = mrz.Substring(18, 1);
                }

                // 提取有效期（YYMMDD）
                if (mrz.Length >= 25)
                {
                    string expiry = mrz.Substring(19, 6);
                    result.DateOfExpiry = ParseDate(expiry);
                }

                result.IsValid = !string.IsNullOrEmpty(result.DocumentNumber) || !string.IsNullOrEmpty(result.FullName);
            }
            catch
            {
                result.IsValid = false;
            }
        }

        /// <summary>
        /// 解析 TD3 格式（2 行 x 44 字符）
        /// </summary>
        private static void ParseTd3Format(string mrz, MrzResult result)
        {
            try
            {
                // 提取第 1 行姓名（从位置 5 开始）
                if (mrz.Length >= 44)
                {
                    string nameLine = mrz.Substring(5, 39);
                    ParseNameFromMrz(nameLine, result);
                }

                // 提取第 2 行数据
                if (mrz.Length >= 88)
                {
                    string line2 = mrz.Substring(44, 44);

                    // 证件号码（0-8）
                    result.DocumentNumber = line2.Substring(0, 9).TrimEnd('<').TrimEnd('0');

                    // 国籍（11-13）
                    result.Nationality = line2.Substring(11, 3);

                    // 出生日期（14-19）
                    string dob = line2.Substring(14, 6);
                    result.DateOfBirth = ParseDate(dob);

                    // 性别（22-23）
                    result.Sex = line2.Substring(22, 1);

                    // 有效期（24-29）
                    string expiry = line2.Substring(24, 6);
                    result.DateOfExpiry = ParseDate(expiry);
                }

                result.IsValid = !string.IsNullOrEmpty(result.DocumentNumber) || !string.IsNullOrEmpty(result.FullName);
            }
            catch
            {
                result.IsValid = false;
            }
        }

        /// <summary>
        /// 通用格式解析
        /// </summary>
        private static void ParseGenericFormat(string mrz, MrzResult result)
        {
            // 尝试查找姓名（通常包含<<）
            int nameStart = mrz.IndexOf("<<");
            if (nameStart > 0)
            {
                int nameEnd = mrz.IndexOf("<<", nameStart + 2);
                if (nameEnd < 0) nameEnd = mrz.Length;
                string namePart = mrz.Substring(nameStart, nameEnd - nameStart);
                ParseNameFromMrz(namePart, result);
            }

            // 尝试查找证件号码（字母 + 数字组合）
            Match match = Regex.Match(mrz, @"[A-Z]{1,2}\d{6,9}");
            if (match.Success)
            {
                result.DocumentNumber = match.Value;
            }

            result.IsValid = !string.IsNullOrEmpty(result.DocumentNumber) || !string.IsNullOrEmpty(result.FullName);
        }

        /// <summary>
        /// 从 MRZ 解析姓名
        /// </summary>
        private static void ParseNameFromMrz(string nameLine, MrzResult result)
        {
            // MRZ 姓名格式：SURNAME<<GIVEN<NAMES>
            nameLine = nameLine.Replace("<", " ").Trim();
            
            // 分割姓和名（多个空格分隔）
            string[] parts = nameLine.Split(new[] { "  " }, StringSplitOptions.None);
            
            if (parts.Length >= 2)
            {
                result.Surname = parts[0].Trim();
                result.GivenNames = string.Join(" ", parts[1..]).Trim();
                result.FullName = $"{result.Surname} {result.GivenNames}";
            }
            else if (parts.Length == 1)
            {
                result.FullName = parts[0].Trim();
                result.Surname = parts[0].Trim();
            }
        }

        /// <summary>
        /// 解析日期（YYMMDD -> YYYY-MM-DD）
        /// </summary>
        private static string ParseDate(string dateStr)
        {
            if (dateStr.Length != 6 || !int.TryParse(dateStr, out int dateVal))
            {
                return "";
            }

            int year = int.Parse(dateStr.Substring(0, 2));
            int month = int.Parse(dateStr.Substring(2, 2));
            int day = int.Parse(dateStr.Substring(4, 2));

            // 判断世纪（简单规则：<=30 为 20xx，>30 为 19xx）
            int fullYear = year <= 30 ? 2000 + year : 1900 + year;

            return $"{fullYear}-{month:D2}-{day:D2}";
        }
    }
}
