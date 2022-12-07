package toolgood.flowVision.ThirdParty.UnitConversion;

import toolgood.flowVision.ThirdParty.UnitConversion.Base.BaseUnitConverter;
import toolgood.flowVision.ThirdParty.UnitConversion.Base.UnitFactorSynonyms;
import toolgood.flowVision.ThirdParty.UnitConversion.Base.UnitFactors;

public class MassConverter extends BaseUnitConverter {

    static UnitFactors units;

    static {
        units = new UnitFactors("kg");
        units.put(new UnitFactorSynonyms(new String[]{"kg", "kilogram","千克"}), 1d);
        units.put(new UnitFactorSynonyms(new String[]{"gram", "g","克"}), 1000d);
        units.put(new UnitFactorSynonyms(new String[]{"吨", "t"}), 1/ 1000d);
        units.put(new UnitFactorSynonyms(new String[]{"lb", "lbs", "pound", "pounds","英镑"}), 100000000d / 45359237);
        units.put(new UnitFactorSynonyms(new String[]{"st", "stone","石"}),  50000000d / 317514659);
        units.put(new UnitFactorSynonyms(new String[]{"oz", "ounce","盎司"}), 1600000000d / 45359237);
        units.put(new UnitFactorSynonyms(new String[]{"quintal","英担"}), 0.01d);
        units.put(new UnitFactorSynonyms(new String[]{"short ton", "net ton", "us ton","短吨","美吨"}), 0.00110231d);
        units.put(new UnitFactorSynonyms(new String[]{"long ton", "weight ton", "gross ton", "imperial ton","长吨","英吨"}), 0.000984207d);


    }


    public MassConverter(String leftUnit, String rightUnit) {
        Instantiate(units, leftUnit, rightUnit);
    }

    public MassConverter() {
        Instantiate(units);
    }

    public static Boolean Exists(String leftSynonym, String rightSynonym) {
        if (units.FindUnit(leftSynonym) != null) {
            return units.FindUnit(rightSynonym) != null;
        }
        return false;
    }

}
