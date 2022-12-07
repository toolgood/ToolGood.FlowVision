﻿//-----------------------------------------------------------------------
// <copyright file="UnitFactorSynonyms.cs" company="George Kampolis">
//     Copyright (c) George Kampolis. All rights reserved.
//     Licensed under the MIT License, Version 2.0. See LICENSE.md in the project root for license information.
// </copyright>
//-----------------------------------------------------------------------

namespace UnitConversion.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class UnitFactorSynonyms
    {
        public UnitFactorSynonyms() { }
        public UnitFactorSynonyms(params string[] items)
        {
            _synonyms.AddRange(items);
        }

        List<string> _synonyms = new List<string>();

        /// <summary>
        /// The list of synonyms this object holds
        /// </summary>
        public IEnumerable<string> Synonyms
        {
            get
            {
                return _synonyms.AsEnumerable();
            }
        }


        // ** INTERNAL HELPERS **

        // Append new syonym to the list
        internal void AddSynonym(string synonym)
        {
            if (Contains(synonym))
            {
                throw new UnitAlreadyExistsException(synonym);
            }
            _synonyms.Add(synonym);
        }

        // Find if some synonym of a given UnitFactor is included in this UnitFactor
        internal bool Contains(UnitFactorSynonyms synonyms)
        {
            foreach (var syn in synonyms.Synonyms)
            {
                if (this.Contains(syn))
                {
                    return true;
                }
            }
            return false;
        }

        // Find if some synonym is included in this UnitFactor
        internal bool Contains(string synonym)
        {
            return _synonyms.Contains(synonym, StringComparer.CurrentCultureIgnoreCase);
        }


        // ** OVERRIDES **
        
        public override int GetHashCode()
        {
            return _synonyms.GetHashCode();
        }

        public override string ToString()
        {
            return String.Join(", ", _synonyms);
        }

        // Allow strings to be interpreted as a UnitDictionaryKey
        public static implicit operator UnitFactorSynonyms(string synonym)
        {
            return new UnitFactorSynonyms(synonym);
        }
    }
}
