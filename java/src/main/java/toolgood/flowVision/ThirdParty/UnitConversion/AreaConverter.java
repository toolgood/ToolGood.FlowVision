package toolgood.flowVision.ThirdParty.UnitConversion;

import toolgood.flowVision.ThirdParty.UnitConversion.Base.BaseUnitConverter;
import toolgood.flowVision.ThirdParty.UnitConversion.Base.UnitFactorSynonyms;
import toolgood.flowVision.ThirdParty.UnitConversion.Base.UnitFactors;

public class AreaConverter extends BaseUnitConverter {
    static UnitFactors units;

    static {
        units = new UnitFactors("m²");
        units.put(new UnitFactorSynonyms(new String[]{"m²", "m2", "square metre", "centiare", "平方米", "平方公尺"}), 1d);
        units.put(new UnitFactorSynonyms(new String[]{"km²", "km2", "kilometre","平方千米"}), 0.000001d);
        units.put(new UnitFactorSynonyms(new String[]{"cm²", "cm2", "centimetre","平方厘米"}), 10000d);
        units.put(new UnitFactorSynonyms(new String[]{"mm²", "mm2", "millimetre","平方毫米"}), 1000000d);
        units.put(new UnitFactorSynonyms(new String[]{"ft²", "ft2", "square foot", "square feet", "sq ft","平方英尺"}),  1d /  0.3048d /  0.3048d);
        units.put(new UnitFactorSynonyms(new String[]{"yd²", "yd2", "sq yd", "square yard","平方码"}), 1d /  0.9144d /  0.9144d);
        units.put(new UnitFactorSynonyms(new String[]{"a", "are"}), 0.01d);
        units.put(new UnitFactorSynonyms(new String[]{"ha", "hectare","公顷"}), 0.0001d);
        units.put(new UnitFactorSynonyms(new String[]{"in²", "in2", "sq in", "square inch","平方英寸"}), 1d / 0.00064516d );
        units.put(new UnitFactorSynonyms(new String[]{"mi²", "mi2", "sq mi", "square mile","平方英里"}), 1d / 2589988.110336d);
        units.put(new UnitFactorSynonyms(new String[]{"亩"}),  1d / 666.667d);
    }


    public AreaConverter(String leftUnit, String rightUnit) {
        Instantiate(units, leftUnit, rightUnit);
    }

    public AreaConverter() {
        Instantiate(units);
    }

    public static Boolean Exists(String leftSynonym, String rightSynonym) {
        if (units.FindUnit(leftSynonym) != null) {
            return units.FindUnit(rightSynonym) != null;
        }
        return false;
    }

}
