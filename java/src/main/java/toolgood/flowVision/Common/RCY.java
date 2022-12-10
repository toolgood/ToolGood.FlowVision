package toolgood.flowVision.Common;

public class RCY {
    private static final int keyLen = 256;

    public static byte[] Encrypt(byte[] data, byte[] pass) throws Exception {
        if (data == null) throw new Exception("data");
        if (data.length == 0) throw new Exception("data");
        if (pass == null) throw new Exception("pass");
        if (pass.length == 0) throw new Exception("pass");

        return encrypt(data, pass);
    }

    private static byte[] encrypt(byte[] data, byte[] pass) {
        byte[] mBox = GetKey(pass, keyLen);
        byte[] output = new byte[data.length];
        int i = 0, j = 0;
        byte a, c;
        for (int offset = 0; offset < data.length; offset++) {
            i = (++i) & 0xFF;
            j = (j + (mBox[i]& 0xff)) & 0xFF;

            a = data[offset];
            c = (byte) (a ^ mBox[((mBox[i] & 0xff) & (mBox[j] & 0xff))]);
            output[offset] = c;

            j = j + (a& 0xff) + (c& 0xff);
        }
        return output;
    }


    private static byte[] GetKey(byte[] pass, int kLen) {
        byte[] mBox = new byte[kLen];

        for (int i = 0; i < kLen; i++) {
            mBox[i] = (byte) i;
        }
        int j = 0;
        for (int i = 0; i < kLen; i++) {
            j = (j + (mBox[i] & 0xff) + pass[i % pass.length]) % kLen;
            byte temp = mBox[i];
            mBox[i] = mBox[j];
            mBox[j] = temp;
        }
        return mBox;
    }
}
