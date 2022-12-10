package toolgood.flowVision.Common;

import javax.crypto.BadPaddingException;
import javax.crypto.Cipher;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.NoSuchPaddingException;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.security.*;
import java.security.spec.InvalidKeySpecException;
import java.security.spec.PKCS8EncodedKeySpec;
import java.security.spec.X509EncodedKeySpec;
import java.util.Base64;

public class RsaUtil {

    private static final String CHARSET = "utf-8";
    private static final Base64.Decoder decoder64 = Base64.getDecoder();
    private static final Base64.Encoder encoder64 = Base64.getEncoder();

    /**
     * 生成公私钥
     * @param keySize
     * @return
     * @throws NoSuchAlgorithmException
     */
    public static SecretKey generateSecretKey(int keySize) throws NoSuchAlgorithmException {
        //生成密钥对
        KeyPairGenerator keyGen = KeyPairGenerator.getInstance("RSA");
        keyGen.initialize(keySize, new SecureRandom());
        KeyPair pair = keyGen.generateKeyPair();
        PrivateKey privateKey = pair.getPrivate();
        PublicKey publicKey = pair.getPublic();
        //这里可以将密钥对保存到本地
        return new SecretKey(encoder64.encodeToString(publicKey.getEncoded()), encoder64.encodeToString(privateKey.getEncoded()));
    }
    /**
     * 私钥加密
     * @param data
     * @param privateInfoStr
     * @return
     * @throws IOException
     */
    public static String encryptData(String data, String privateInfoStr) throws IOException, InvalidKeySpecException, NoSuchAlgorithmException, InvalidKeyException, NoSuchPaddingException, BadPaddingException, IllegalBlockSizeException {

        Cipher cipher = Cipher.getInstance("RSA/ECB/PKCS1Padding");
        cipher.init(Cipher.ENCRYPT_MODE, getPrivateKey(privateInfoStr));
        return encoder64.encodeToString(cipher.doFinal(data.getBytes(CHARSET)));
    }

    /**
     * 公钥解密
     * @param data
     * @param publicInfoStr
     * @return
     */
    public static String decryptData(String data, String publicInfoStr) throws NoSuchPaddingException, NoSuchAlgorithmException, InvalidKeySpecException, InvalidKeyException, BadPaddingException, IllegalBlockSizeException, UnsupportedEncodingException {
        byte[] encryptDataBytes=decoder64.decode(data.getBytes(CHARSET));
        //解密
        Cipher cipher = Cipher.getInstance("RSA/ECB/PKCS1Padding");
        cipher.init(Cipher.DECRYPT_MODE, getPublicKey(publicInfoStr));
        return new String(cipher.doFinal(encryptDataBytes), CHARSET);
    }
    private static PublicKey getPublicKey(String base64PublicKey) throws NoSuchAlgorithmException, InvalidKeySpecException {
        X509EncodedKeySpec keySpec = new X509EncodedKeySpec(Base64.getDecoder().decode(base64PublicKey.getBytes()));
        KeyFactory keyFactory = KeyFactory.getInstance("RSA");
        return keyFactory.generatePublic(keySpec);
    }
    private static PrivateKey getPrivateKey(String base64PrivateKey) throws NoSuchAlgorithmException, InvalidKeySpecException {
        PrivateKey privateKey = null;
        PKCS8EncodedKeySpec keySpec = new PKCS8EncodedKeySpec(Base64.getDecoder().decode(base64PrivateKey.getBytes()));
        KeyFactory keyFactory = null;
        keyFactory = KeyFactory.getInstance("RSA");
        privateKey = keyFactory.generatePrivate(keySpec);
        return privateKey;
    }

    /**
     * 密钥实体
     * @author hank
     * @since 2020/2/28 0028 下午 16:27
     */
    public static class SecretKey {
        /**
         * 公钥
         */
        private String publicKey;
        /**
         * 私钥
         */
        private String privateKey;

        public SecretKey(String publicKey, String privateKey) {
            this.publicKey = publicKey;
            this.privateKey = privateKey;
        }

        public String getPublicKey() {
            return publicKey;
        }

        public void setPublicKey(String publicKey) {
            this.publicKey = publicKey;
        }

        public String getPrivateKey() {
            return privateKey;
        }

        public void setPrivateKey(String privateKey) {
            this.privateKey = privateKey;
        }

        @Override
        public String toString() {
            return "SecretKey{" +
                    "publicKey='" + publicKey + '\'' +
                    ", privateKey='" + privateKey + '\'' +
                    '}';
        }
    }

    private static void writeToFile(String path, byte[] key) throws IOException {
        File f = new File(path);
        f.getParentFile().mkdirs();

        try(FileOutputStream fos = new FileOutputStream(f)) {
            fos.write(key);
            fos.flush();
        }
    }


}
