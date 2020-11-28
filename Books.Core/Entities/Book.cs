using Books.Core.Validations;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace Books.Core.Entities
{
    public class Book : EntityObject, IValidatableObject
    {

        public ICollection<BookAuthor> BookAuthors { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Titel muss definiert sein")]
        [MaxLength(200, ErrorMessage = "Titel darf maximal 200 Zeichen lang sein")]
        public string Title { get; set; }

        [Required, MaxLength(100)]
        public string Publishers { get; set; }

        [IsbnValidation]
        [Required, MaxLength(13)]
        public string Isbn { get; set; }

        public Book()
        {
            BookAuthors = new List<BookAuthor>();
        }

        /// <summary>
        /// Eine gültige ISBN-Nummer besteht aus den Ziffern 0, ... , 9,
        /// 'x' oder 'X' (nur an der letzten Stelle)
        /// Die Gesamtlänge der ISBN beträgt 10 Zeichen.
        /// Für die Ermittlung der Prüfsumme werden die Ziffern 
        /// von rechts nach links mit 1 - 10 multipliziert und die 
        /// Produkte aufsummiert. Ist das rechte Zeichen ein x oder X
        /// wird als Zahlenwert 10 verwendet.
        /// Die Prüfsumme muss modulo 11 0 ergeben.
        /// </summary>
        /// <returns>Prüfergebnis</returns>
        public static bool CheckIsbn(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
            {
                return false;
            }
            if (isbn.Length != 10)
            {
                Debug.WriteLine($"!!! isbn {isbn} has no length of 10!");
                return false;
            }
            int weight = 10;  // Startgewicht
            int sum = 0;
            for (int i = 0; i < 10; i++)
            {
                var ch = isbn[i];
                int number;
                if (char.IsDigit(ch))
                {
                    number = ch - '0';
                }
                else  // keine Ziffer  => x oder X an letzter Stelle
                {
                    if (i != 9)
                    {
                        return false;
                    }
                    if (ch == 'x' || ch == 'X')
                    {
                        number = 10;
                    }
                    else
                    {
                        return false;
                    }
                }
                // zahl enthält gültigen Wert
                sum += number * weight;
                weight--;
            }
            if (sum % 11 != 0)
            {
                Debug.WriteLine($"!!! isbn {isbn} checksum is {sum % 11} (should be 0!");
            }
            return (sum % 11) == 0;
        }


        public override string ToString()
        {
            return $"{Title} {Isbn} mit {BookAuthors.Count()} Autoren";
        }


        /// <inheritdoc />
        /// <summary>
        /// Jedes Buch muss zumindest einen Autor haben.
        /// Weiters darf ein Autor einem Buch nicht mehrfach zugewiesen
        /// werden.
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
            if(BookAuthors.Count == 0)
            {
                yield return new ValidationResult("Book must have at least one author");
            }
            var doubleAuthorNames = BookAuthors
                .GroupBy(ba => ba.Author)
                .Where(baGroup => baGroup.Count() > 1)
                .Select(baGroup => baGroup.Key.Name)
                .ToArray()
                .Join();
            if (doubleAuthorNames.Length > 0)
            {
                yield return new ValidationResult($"{doubleAuthorNames} are twice authors of the book");
            }
        }
  }
}

