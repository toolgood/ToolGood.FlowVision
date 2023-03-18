package toolgood.flowVision.ThirdParty.UnitConversion;

import toolgood.flowVision.ThirdParty.UnitConversion.Base.BaseUnitConverter;
import toolgood.flowVision.ThirdParty.UnitConversion.Base.UnitFactorSynonyms;
import toolgood.flowVision.ThirdParty.UnitConversion.Base.UnitFactors;

import java.math.BigDecimal;

public class DistanceConverter extends BaseUnitConverter {
    static UnitFactors units;

    static {
        units = new UnitFactors("m");
        units.put(new UnitFactorSynonyms(new String[]{"m", "metre", "米"}), new BigDecimal("1"));
        units.put(new UnitFactorSynonyms(new String[]{"km", "kilometre", "千米"}), new BigDecimal("0.001"));
        units.put(new UnitFactorSynonyms(new String[]{"dm", "decimetre", "分米"}), new BigDecimal("10"));
        units.put(new UnitFactorSynonyms(new String[]{"cm", "centimetre", "厘米"}), new BigDecimal("100"));
        units.put(new UnitFactorSynonyms(new String[]{"mm", "millimetre", "毫米"}), new BigDecimal("1000"));
        units.put(new UnitFactorSynonyms(new String[]{"ft", "foot", "feet", "英尺"}), BigDecimal.valueOf(1250d / 381));
        units.put(new UnitFactorSynonyms(new String[]{"yd", "yard", "码"}), BigDecimal.valueOf(1250d / 1143));
        units.put(new UnitFactorSynonyms(new String[]{"mile", "英里"}), BigDecimal.valueOf(125d / 201168));
        units.put(new UnitFactorSynonyms(new String[]{"in", "inch", "英寸"}), BigDecimal.valueOf(5000d / 127));
        units.put(new UnitFactorSynonyms(new String[]{"au"}), BigDecimal.valueOf(1d / 149600000000d));

    }


    public DistanceConverter(String leftUnit, String rightUnit) {
        Instantiate(units, leftUnit, rightUnit);
    }

    public DistanceConverter() {
        Instantiate(units);
    }

    public static Boolean Exists(String leftSynonym, String rightSynonym) {
        if (units.FindUnit(leftSynonym) != null) {
            return units.FindUnit(rightSynonym) != null;
        }
        return false;
    }

}
