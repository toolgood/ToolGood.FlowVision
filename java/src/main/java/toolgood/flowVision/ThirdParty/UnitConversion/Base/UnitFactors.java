package toolgood.flowVision.ThirdParty.UnitConversion.Base;

import java.util.TreeMap;

public class UnitFactors extends TreeMap<UnitFactorSynonyms, Double> {
    public String BaseUnit;

    public UnitFactors(String baseUnit) {
        BaseUnit = baseUnit;
    }

    public UnitFactorSynonyms FindUnit(String synonyms) {
        return FindUnit(new UnitFactorSynonyms(synonyms));
    }

    public UnitFactorSynonyms FindUnit(UnitFactorSynonyms synonyms) {
        for (UnitFactorSynonyms factor : this.keySet()) {
            if (factor.Contains(synonyms)) {
                return factor;
            }
        }
        return null;
    }

    public double FindFactor(String synonyms) throws UnitNotSupportedException {
        return FindFactor(new UnitFactorSynonyms(synonyms));
    }

    // Get the factor for a given unit
    public double FindFactor(UnitFactorSynonyms synonyms) throws UnitNotSupportedException {
        for (UnitFactorSynonyms factor : this.keySet()) {
            if (factor.Contains(synonyms)) {
                return this.get(factor);
            }
        }
        throw new UnitNotSupportedException(synonyms.toString());
    }


}
