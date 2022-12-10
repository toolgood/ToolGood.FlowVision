package toolgood.flowVision.Common;

import java.math.BigInteger;
import java.security.*;
import java.security.interfaces.RSAPublicKey;
import java.security.spec.InvalidKeySpecException;
import java.security.spec.X509EncodedKeySpec;
import java.util.Base64;

public class RsaUtil {

    public static byte[] decryptData(byte[] encryptDataBytes, String publicInfoStr) throws NoSuchAlgorithmException, InvalidKeySpecException {
        RSAPublicKey publicKey = (RSAPublicKey) getPublicKey(publicInfoStr);
        BigInteger data = new BigInteger(encryptDataBytes);
        byte[] dt2 = data.modPow(publicKey.getPublicExponent(), publicKey.getModulus()).toByteArray();
        for (int i = 0; i < dt2.length; i++) {
            byte temp = dt2[i];
            dt2[i] = dt2[dt2.length - 1 - i];
            dt2[dt2.length - 1 - i] = temp;
        }
        return dt2;
    }

    private static PublicKey getPublicKey(String base64PublicKey) throws NoSuchAlgorithmException, InvalidKeySpecException {
        X509EncodedKeySpec keySpec = new X509EncodedKeySpec(Base64.getDecoder().decode(base64PublicKey.getBytes()));
        KeyFactory keyFactory = KeyFactory.getInstance("RSA");
        return keyFactory.generatePublic(keySpec);
    }

}
