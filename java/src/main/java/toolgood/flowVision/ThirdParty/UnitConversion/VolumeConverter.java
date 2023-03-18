package toolgood.flowVision.ThirdParty.UnitConversion;

import toolgood.flowVision.ThirdParty.UnitConversion.Base.BaseUnitConverter;
import toolgood.flowVision.ThirdParty.UnitConversion.Base.UnitFactorSynonyms;
import toolgood.flowVision.ThirdParty.UnitConversion.Base.UnitFactors;

import java.math.BigDecimal;

public class VolumeConverter extends BaseUnitConverter {
    static UnitFactors units;

    static {
        units = new UnitFactors("l");
        units.put(new UnitFactorSynonyms(new String[]{"l", "L", "lt", "ltr", "liter", "litre", "dm³", "dm3", "cubic decimetre", "升", "立方分米"}), new BigDecimal(1d));
        units.put(new UnitFactorSynonyms(new String[]{"m³", "m3", "cubic metre", "立方米"}), new BigDecimal(0.001d));
        units.put(new UnitFactorSynonyms(new String[]{"cm³", "cm3", "cubic centimetre", "立方厘米", "毫升"}), new BigDecimal(1000d));
        units.put(new UnitFactorSynonyms(new String[]{"mm³", "mm3", "cubic millimetre", "立方毫米"}), new BigDecimal(1000000d));
        units.put(new UnitFactorSynonyms(new String[]{"ft³", "ft3", "cubic foot", "cubic feet", "cu ft", "立方英尺"}),new BigDecimal( 0.0353147d));
        units.put(new UnitFactorSynonyms(new String[]{"in³", "in3", "cu in", "cubic inch", "立方英寸"}), new BigDecimal(61.0237d));
        units.put(new UnitFactorSynonyms(new String[]{"imperial pint", "imperial pt", "imperial p"}), new BigDecimal(1.75975d));
        units.put(new UnitFactorSynonyms(new String[]{"imperial gallon", "imperial gal"}), new BigDecimal(0.219969d));
        units.put(new UnitFactorSynonyms(new String[]{"imperial quart", "imperial qt"}),new BigDecimal( 0.879877d));
        units.put(new UnitFactorSynonyms(new String[]{"US pint", "US pt", "US p"}), new BigDecimal(2.11337643513819d));
        units.put(new UnitFactorSynonyms(new String[]{"US gallon", "US gal"}), new BigDecimal(0.264172d));
        units.put(new UnitFactorSynonyms(new String[]{"US quart", "US qt"}),new BigDecimal( 2.11338d));
    }


    public VolumeConverter(String leftUnit, String rightUnit) {
        Instantiate(units, leftUnit, rightUnit);
    }

    public VolumeConverter() {
        Instantiate(units);
    }

    public static Boolean Exists(String leftSynonym, String rightSynonym) {
        if (units.FindUnit(leftSynonym) != null) {
            return units.FindUnit(rightSynonym) != null;
        }
        return false;
    }

}
