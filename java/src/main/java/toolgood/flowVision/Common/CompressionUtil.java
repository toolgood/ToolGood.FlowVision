package toolgood.flowVision.Common;


import com.aayushatharva.brotli4j.Brotli4jLoader;
import com.aayushatharva.brotli4j.decoder.Decoder;
import com.aayushatharva.brotli4j.decoder.DirectDecompress;

import java.io.IOException;

public class CompressionUtil {

    static {
        Brotli4jLoader.ensureAvailability();
    }

    public static byte[] BrDecompress(byte[] data) throws IOException {
        DirectDecompress directDecompress = Decoder.decompress(data);
        return directDecompress.getDecompressedData();
    }

}
