(function (factory) {
    if (typeof define === 'function' && define.amd) { // AMD. Register as an anonymous module.
        define(['jquery'], factory);
    } else if (typeof exports === 'object') { // Node/CommonJS
        var jQuery = require('jquery');
        module.exports = factory(jQuery);
    } else { // Browser globals (zepto supported)
        factory(window.jQuery || window.Zepto || window.$); // Zepto supported on browsers as well
    }
}(function ($) {
    function guid() { function getGUIDDate() { return date.getFullYear() + addZero(date.getMonth() + 1) + addZero(date.getDay()) } function getGUIDTime() { return addZero(date.getHours()) + addZero(date.getMinutes()) + addZero(date.getSeconds()) + addZero(parseInt(date.getMilliseconds() / 10)) } function addZero(num) { if (Number(num).toString() != "NaN" && num >= 0 && num < 10) { return "0" + Math.floor(num) } else { return num.toString() } } function hexadecimal(num, x, y) { if (y != undefined) { return parseInt(num.toString(), y).toString(x) } else { return parseInt(num.toString()).toString(x) } } function formatGUID(guidStr) { var str1 = guidStr.slice(0, 8) + "-", str2 = guidStr.slice(8, 12) + "-", str3 = guidStr.slice(12, 16) + "-", str4 = guidStr.slice(16, 20) + "-", str5 = guidStr.slice(20); return str1 + str2 + str3 + str4 + str5 } date = new Date(); date = new Date(); var guidStr = ""; sexadecimalDate = hexadecimal(getGUIDDate(), 16); sexadecimalTime = hexadecimal(getGUIDTime(), 16); guidStr += sexadecimalDate; guidStr += sexadecimalTime; for (var i = 0; i < 9; i++) { guidStr += Math.floor(Math.random() * 16).toString(16) } while (guidStr.length < 32) { guidStr += Math.floor(Math.random() * 16).toString(16) } return formatGUID(guidStr) };

    var chrsz = 8;
    var dbits; var canary = 244837814094590; var j_lm = ((canary & 16777215) == 15715070);
    var BI_FP = 52;
    var BI_RM = "0123456789abcdefghijklmnopqrstuvwxyz"; var BI_RC = new Array(); var rr, vv; rr = "0".charCodeAt(0);
    for (vv = 0; vv <= 9; ++vv) { BI_RC[rr++] = vv } rr = "a".charCodeAt(0); for (vv = 10; vv < 36; ++vv) { BI_RC[rr++] = vv } rr = "A".charCodeAt(0); for (vv = 10; vv < 36; ++vv) { BI_RC[rr++] = vv }
    var rng_psize = 256; var rng_state; var rng_pool; var rng_pptr;
    var b64map = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"; var b64padchar = "=";

    function md5(A) { return binl2hex(core_md5(str2binl(A), A.length * chrsz)) }
    function core_md5(I, G) { I[G >> 5] |= 128 << ((G) % 32); I[(((G + 64) >>> 9) << 4) + 14] = G; var H = 1732584193; var F = -271733879; var J = -1732584194; var K = 271733878; for (var C = 0; C < I.length; C += 16) { var D = H; var E = F; var A = J; var B = K; H = md5_ff(H, F, J, K, I[C + 0], 7, -680876936); K = md5_ff(K, H, F, J, I[C + 1], 12, -389564586); J = md5_ff(J, K, H, F, I[C + 2], 17, 606105819); F = md5_ff(F, J, K, H, I[C + 3], 22, -1044525330); H = md5_ff(H, F, J, K, I[C + 4], 7, -176418897); K = md5_ff(K, H, F, J, I[C + 5], 12, 1200080426); J = md5_ff(J, K, H, F, I[C + 6], 17, -1473231341); F = md5_ff(F, J, K, H, I[C + 7], 22, -45705983); H = md5_ff(H, F, J, K, I[C + 8], 7, 1770035416); K = md5_ff(K, H, F, J, I[C + 9], 12, -1958414417); J = md5_ff(J, K, H, F, I[C + 10], 17, -42063); F = md5_ff(F, J, K, H, I[C + 11], 22, -1990404162); H = md5_ff(H, F, J, K, I[C + 12], 7, 1804603682); K = md5_ff(K, H, F, J, I[C + 13], 12, -40341101); J = md5_ff(J, K, H, F, I[C + 14], 17, -1502002290); F = md5_ff(F, J, K, H, I[C + 15], 22, 1236535329); H = md5_gg(H, F, J, K, I[C + 1], 5, -165796510); K = md5_gg(K, H, F, J, I[C + 6], 9, -1069501632); J = md5_gg(J, K, H, F, I[C + 11], 14, 643717713); F = md5_gg(F, J, K, H, I[C + 0], 20, -373897302); H = md5_gg(H, F, J, K, I[C + 5], 5, -701558691); K = md5_gg(K, H, F, J, I[C + 10], 9, 38016083); J = md5_gg(J, K, H, F, I[C + 15], 14, -660478335); F = md5_gg(F, J, K, H, I[C + 4], 20, -405537848); H = md5_gg(H, F, J, K, I[C + 9], 5, 568446438); K = md5_gg(K, H, F, J, I[C + 14], 9, -1019803690); J = md5_gg(J, K, H, F, I[C + 3], 14, -187363961); F = md5_gg(F, J, K, H, I[C + 8], 20, 1163531501); H = md5_gg(H, F, J, K, I[C + 13], 5, -1444681467); K = md5_gg(K, H, F, J, I[C + 2], 9, -51403784); J = md5_gg(J, K, H, F, I[C + 7], 14, 1735328473); F = md5_gg(F, J, K, H, I[C + 12], 20, -1926607734); H = md5_hh(H, F, J, K, I[C + 5], 4, -378558); K = md5_hh(K, H, F, J, I[C + 8], 11, -2022574463); J = md5_hh(J, K, H, F, I[C + 11], 16, 1839030562); F = md5_hh(F, J, K, H, I[C + 14], 23, -35309556); H = md5_hh(H, F, J, K, I[C + 1], 4, -1530992060); K = md5_hh(K, H, F, J, I[C + 4], 11, 1272893353); J = md5_hh(J, K, H, F, I[C + 7], 16, -155497632); F = md5_hh(F, J, K, H, I[C + 10], 23, -1094730640); H = md5_hh(H, F, J, K, I[C + 13], 4, 681279174); K = md5_hh(K, H, F, J, I[C + 0], 11, -358537222); J = md5_hh(J, K, H, F, I[C + 3], 16, -722521979); F = md5_hh(F, J, K, H, I[C + 6], 23, 76029189); H = md5_hh(H, F, J, K, I[C + 9], 4, -640364487); K = md5_hh(K, H, F, J, I[C + 12], 11, -421815835); J = md5_hh(J, K, H, F, I[C + 15], 16, 530742520); F = md5_hh(F, J, K, H, I[C + 2], 23, -995338651); H = md5_ii(H, F, J, K, I[C + 0], 6, -198630844); K = md5_ii(K, H, F, J, I[C + 7], 10, 1126891415); J = md5_ii(J, K, H, F, I[C + 14], 15, -1416354905); F = md5_ii(F, J, K, H, I[C + 5], 21, -57434055); H = md5_ii(H, F, J, K, I[C + 12], 6, 1700485571); K = md5_ii(K, H, F, J, I[C + 3], 10, -1894986606); J = md5_ii(J, K, H, F, I[C + 10], 15, -1051523); F = md5_ii(F, J, K, H, I[C + 1], 21, -2054922799); H = md5_ii(H, F, J, K, I[C + 8], 6, 1873313359); K = md5_ii(K, H, F, J, I[C + 15], 10, -30611744); J = md5_ii(J, K, H, F, I[C + 6], 15, -1560198380); F = md5_ii(F, J, K, H, I[C + 13], 21, 1309151649); H = md5_ii(H, F, J, K, I[C + 4], 6, -145523070); K = md5_ii(K, H, F, J, I[C + 11], 10, -1120210379); J = md5_ii(J, K, H, F, I[C + 2], 15, 718787259); F = md5_ii(F, J, K, H, I[C + 9], 21, -343485551); H = safe_add(H, D); F = safe_add(F, E); J = safe_add(J, A); K = safe_add(K, B) } return Array(H, F, J, K) }
    function md5_cmn(B, F, C, D, A, E) { return safe_add(bit_rol(safe_add(safe_add(F, B), safe_add(D, E)), A), C) }
    function md5_ff(F, C, D, A, E, B, G) { return md5_cmn((C & D) | ((~C) & A), F, C, E, B, G) }
    function md5_gg(F, C, D, A, E, B, G) { return md5_cmn((C & A) | (D & (~A)), F, C, E, B, G) }
    function md5_hh(F, C, D, A, E, B, G) { return md5_cmn(C ^ D ^ A, F, C, E, B, G) }
    function md5_ii(F, C, D, A, E, B, G) { return md5_cmn(D ^ (C | (~A)), F, C, E, B, G) }
    function safe_add(B, C) { var A = (B & 65535) + (C & 65535); var D = (B >> 16) + (C >> 16) + (A >> 16); return (D << 16) | (A & 65535) }
    function bit_rol(B, A) { return (B << A) | (B >>> (32 - A)) }
    function str2binl(B) { var C = Array(); var D = (1 << chrsz) - 1; for (var A = 0; A < B.length * chrsz; A += chrsz) { C[A >> 5] |= (B.charCodeAt(A / chrsz) & D) << (A % 32) } return C }
    function binl2hex(C) { var A = "0123456789abcdef"; var D = ""; for (var B = 0; B < C.length * 4; B++) { D += A.charAt((C[B >> 2] >> ((B % 4) * 8 + 4)) & 15) + A.charAt((C[B >> 2] >> ((B % 4) * 8)) & 15) } return D }

    function BigInteger(E, F, D) { if (E != null) { if ("number" == typeof E) { this.fromNumber(E, F, D) } else { if (F == null && "string" != typeof E) { this.fromString(E, 256) } else { this.fromString(E, F) } } } }
    function nbi() { return new BigInteger(null) }
    function am1(L, K, H, I, N, M) { while (--M >= 0) { var J = K * this[L++] + H[I] + N; N = Math.floor(J / 67108864); H[I++] = J & 67108863 } return N }
    function am2(U, V, O, P, L, R) { var Q = V & 32767, T = V >> 15; while (--R >= 0) { var M = this[U] & 32767; var S = this[U++] >> 15; var N = T * M + S * Q; M = Q * M + ((N & 32767) << 15) + O[P] + (L & 1073741823); L = (M >>> 30) + (N >>> 15) + T * S + (L >>> 30); O[P++] = M & 1073741823 } return L }
    function am3(U, V, O, P, L, R) { var Q = V & 16383, T = V >> 14; while (--R >= 0) { var M = this[U] & 16383; var S = this[U++] >> 14; var N = T * M + S * Q; M = Q * M + ((N & 16383) << 14) + O[P] + L; L = (M >> 28) + (N >> 14) + T * S; O[P++] = M & 268435455 } return L } if (j_lm && (navigator.appName == "Microsoft Internet Explorer")) { BigInteger.prototype.am = am2; dbits = 30 } else { if (j_lm && (navigator.appName != "Netscape")) { BigInteger.prototype.am = am1; dbits = 26 } else { BigInteger.prototype.am = am3; dbits = 28 } }
    BigInteger.prototype.DB = dbits;
    BigInteger.prototype.DM = ((1 << dbits) - 1);
    BigInteger.prototype.DV = (1 << dbits);
    BigInteger.prototype.FV = Math.pow(2, BI_FP);
    BigInteger.prototype.F1 = BI_FP - dbits;
    BigInteger.prototype.F2 = 2 * dbits - BI_FP;
    function int2char(B) { return BI_RM.charAt(B) }
    function intAt(D, F) { var E = BI_RC[D.charCodeAt(F)]; return (E == null) ? -1 : E }
    function bnpCopyTo(D) { for (var C = this.t - 1; C >= 0; --C) { D[C] = this[C] } D.t = this.t; D.s = this.s }
    function bnpFromInt(B) { this.t = 1; this.s = (B < 0) ? -1 : 0; if (B > 0) { this[0] = B } else { if (B < -1) { this[0] = B + this.DV } else { this.t = 0 } } }
    function nbv(D) { var C = nbi(); C.fromInt(D); return C }
    function bnpFromString(L, J) { var I; if (J == 16) { I = 4 } else { if (J == 8) { I = 3 } else { if (J == 256) { I = 8 } else { if (J == 2) { I = 1 } else { if (J == 32) { I = 5 } else { if (J == 4) { I = 2 } else { this.fromRadix(L, J); return } } } } } } this.t = 0; this.s = 0; var N = L.length, H = false, M = 0; while (--N >= 0) { var K = (I == 8) ? L[N] & 255 : intAt(L, N); if (K < 0) { if (L.charAt(N) == "-") { H = true } continue } H = false; if (M == 0) { this[this.t++] = K } else { if (M + I > this.DB) { this[this.t - 1] |= (K & ((1 << (this.DB - M)) - 1)) << M; this[this.t++] = (K >> (this.DB - M)) } else { this[this.t - 1] |= K << M } } M += I; if (M >= this.DB) { M -= this.DB } } if (I == 8 && (L[0] & 128) != 0) { this.s = -1; if (M > 0) { this[this.t - 1] |= ((1 << (this.DB - M)) - 1) << M } } this.clamp(); if (H) { BigInteger.ZERO.subTo(this, this) } }
    function bnpClamp() { var B = this.s & this.DM; while (this.t > 0 && this[this.t - 1] == B) { --this.t } }
    function bnToString(N) { if (this.s < 0) { return "-" + this.negate().toString(N) } var O; if (N == 16) { O = 4 } else { if (N == 8) { O = 3 } else { if (N == 2) { O = 1 } else { if (N == 32) { O = 5 } else { if (N == 4) { O = 2 } else { return this.toRadix(N) } } } } } var K = (1 << O) - 1, I, M = false, L = "", P = this.t; var J = this.DB - (P * this.DB) % O; if (P-- > 0) { if (J < this.DB && (I = this[P] >> J) > 0) { M = true; L = int2char(I) } while (P >= 0) { if (J < O) { I = (this[P] & ((1 << J) - 1)) << (O - J); I |= this[--P] >> (J += this.DB - O) } else { I = (this[P] >> (J -= O)) & K; if (J <= 0) { J += this.DB; --P } } if (I > 0) { M = true } if (M) { L += int2char(I) } } } return M ? L : "0" }
    function bnNegate() { var B = nbi(); BigInteger.ZERO.subTo(this, B); return B }
    function bnAbs() { return (this.s < 0) ? this.negate() : this }
    function bnCompareTo(E) { var F = this.s - E.s; if (F != 0) { return F } var D = this.t; F = D - E.t; if (F != 0) { return (this.s < 0) ? -F : F } while (--D >= 0) { if ((F = this[D] - E[D]) != 0) { return F } } return 0 }
    function nbits(D) { var F = 1, E; if ((E = D >>> 16) != 0) { D = E; F += 16 } if ((E = D >> 8) != 0) { D = E; F += 8 } if ((E = D >> 4) != 0) { D = E; F += 4 } if ((E = D >> 2) != 0) { D = E; F += 2 } if ((E = D >> 1) != 0) { D = E; F += 1 } return F }
    function bnBitLength() { if (this.t <= 0) { return 0 } return this.DB * (this.t - 1) + nbits(this[this.t - 1] ^ (this.s & this.DM)) }
    function bnpDLShiftTo(D, F) { var E; for (E = this.t - 1; E >= 0; --E) { F[E + D] = this[E] } for (E = D - 1; E >= 0; --E) { F[E] = 0 } F.t = this.t + D; F.s = this.s }
    function bnpDRShiftTo(D, F) { for (var E = D; E < this.t; ++E) { F[E - D] = this[E] } F.t = Math.max(this.t - D, 0); F.s = this.s }
    function bnpLShiftTo(N, L) { var J = N % this.DB; var P = this.DB - J; var K = (1 << P) - 1; var O = Math.floor(N / this.DB), I = (this.s << J) & this.DM, M; for (M = this.t - 1; M >= 0; --M) { L[M + O + 1] = (this[M] >> P) | I; I = (this[M] & K) << J } for (M = O - 1; M >= 0; --M) { L[M] = 0 } L[O] = I; L.t = this.t + O + 1; L.s = this.s; L.clamp() }
    function bnpRShiftTo(M, L) { L.s = this.s; var N = Math.floor(M / this.DB); if (N >= this.t) { L.t = 0; return } var K = M % this.DB; var I = this.DB - K; var H = (1 << K) - 1; L[0] = this[N] >> K; for (var J = N + 1; J < this.t; ++J) { L[J - N - 1] |= (this[J] & H) << I; L[J - N] = this[J] >> K } if (K > 0) { L[this.t - N - 1] |= (this.s & H) << I } L.t = this.t - N; L.clamp() }
    function bnpSubTo(G, J) { var H = 0, F = 0, I = Math.min(G.t, this.t); while (H < I) { F += this[H] - G[H]; J[H++] = F & this.DM; F >>= this.DB } if (G.t < this.t) { F -= G.s; while (H < this.t) { F += this[H]; J[H++] = F & this.DM; F >>= this.DB } F += this.s } else { F += this.s; while (H < G.t) { F -= G[H]; J[H++] = F & this.DM; F >>= this.DB } F -= G.s } J.s = (F < 0) ? -1 : 0; if (F < -1) { J[H++] = this.DV + F } else { if (F > 0) { J[H++] = F } } J.t = H; J.clamp() }
    function bnpMultiplyTo(G, J) { var I = this.abs(), F = G.abs(); var H = I.t; J.t = H + F.t; while (--H >= 0) { J[H] = 0 } for (H = 0; H < F.t; ++H) { J[H + I.t] = I.am(0, F[H], J, H, 0, I.t) } J.s = 0; J.clamp(); if (this.s != G.s) { BigInteger.ZERO.subTo(J, J) } }
    function bnpSquareTo(H) { var G = this.abs(); var F = H.t = 2 * G.t; while (--F >= 0) { H[F] = 0 } for (F = 0; F < G.t - 1; ++F) { var E = G.am(F, G[F], H, 2 * F, 0, 1); if ((H[F + G.t] += G.am(F + 1, 2 * G[F], H, 2 * F + 1, E, G.t - F - 1)) >= G.DV) { H[F + G.t] -= G.DV; H[F + G.t + 1] = 1 } } if (H.t > 0) { H[H.t - 1] += G.am(F, G[F], H, 2 * F, 0, 1) } H.s = 0; H.clamp() }
    function bnpDivRemTo(a, f, h) { var Y = a.abs(); if (Y.t <= 0) { return } var W = this.abs(); if (W.t < Y.t) { if (f != null) { f.fromInt(0) } if (h != null) { this.copyTo(h) } return } if (h == null) { h = nbi() } var U = nbi(), k = this.s, Z = a.s; var g = this.DB - nbits(Y[Y.t - 1]); if (g > 0) { Y.lShiftTo(g, U); W.lShiftTo(g, h) } else { Y.copyTo(U); W.copyTo(h) } var T = U.t; var X = U[T - 1]; if (X == 0) { return } var V = X * (1 << this.F1) + ((T > 1) ? U[T - 2] >> this.F2 : 0); var d = this.FV / V, b = (1 << this.F1) / V, e = 1 << this.F2; var j = h.t, i = j - T, l = (f == null) ? nbi() : f; U.dlShiftTo(i, l); if (h.compareTo(l) >= 0) { h[h.t++] = 1; h.subTo(l, h) } BigInteger.ONE.dlShiftTo(T, l); l.subTo(U, U); while (U.t < T) { U[U.t++] = 0 } while (--i >= 0) { var c = (h[--j] == X) ? this.DM : Math.floor(h[j] * d + (h[j - 1] + e) * b); if ((h[j] += U.am(0, c, h, i, 0, T)) < c) { U.dlShiftTo(i, l); h.subTo(l, h); while (h[j] < --c) { h.subTo(l, h) } } } if (f != null) { h.drShiftTo(T, f); if (k != Z) { BigInteger.ZERO.subTo(f, f) } } h.t = T; h.clamp(); if (g > 0) { h.rShiftTo(g, h) } if (k < 0) { BigInteger.ZERO.subTo(h, h) } }
    function bnMod(C) { var D = nbi(); this.abs().divRemTo(C, null, D); if (this.s < 0 && D.compareTo(BigInteger.ZERO) > 0) { C.subTo(D, D) } return D }
    function Classic(B) { this.m = B }
    function cConvert(B) { if (B.s < 0 || B.compareTo(this.m) >= 0) { return B.mod(this.m) } else { return B } }
    function cRevert(B) { return B }
    function cReduce(B) { B.divRemTo(this.m, null, B) }
    function cMulTo(D, E, F) { D.multiplyTo(E, F); this.reduce(F) }
    function cSqrTo(C, D) { C.squareTo(D); this.reduce(D) }
    Classic.prototype.convert = cConvert;
    Classic.prototype.revert = cRevert;
    Classic.prototype.reduce = cReduce;
    Classic.prototype.mulTo = cMulTo;
    Classic.prototype.sqrTo = cSqrTo;
    function bnpInvDigit() { if (this.t < 1) { return 0 } var D = this[0]; if ((D & 1) == 0) { return 0 } var C = D & 3; C = (C * (2 - (D & 15) * C)) & 15; C = (C * (2 - (D & 255) * C)) & 255; C = (C * (2 - (((D & 65535) * C) & 65535))) & 65535; C = (C * (2 - D * C % this.DV)) % this.DV; return (C > 0) ? this.DV - C : -C }
    function Montgomery(B) { this.m = B; this.mp = B.invDigit(); this.mpl = this.mp & 32767; this.mph = this.mp >> 15; this.um = (1 << (B.DB - 15)) - 1; this.mt2 = 2 * B.t }
    function montConvert(C) { var D = nbi(); C.abs().dlShiftTo(this.m.t, D); D.divRemTo(this.m, null, D); if (C.s < 0 && D.compareTo(BigInteger.ZERO) > 0) { this.m.subTo(D, D) } return D }
    function montRevert(C) { var D = nbi(); C.copyTo(D); this.reduce(D); return D }
    function montReduce(F) { while (F.t <= this.mt2) { F[F.t++] = 0 } for (var H = 0; H < this.m.t; ++H) { var E = F[H] & 32767; var G = (E * this.mpl + (((E * this.mph + (F[H] >> 15) * this.mpl) & this.um) << 15)) & F.DM; E = H + this.m.t; F[E] += this.m.am(0, G, F, H, 0, this.m.t); while (F[E] >= F.DV) { F[E] -= F.DV; F[++E]++ } } F.clamp(); F.drShiftTo(this.m.t, F); if (F.compareTo(this.m) >= 0) { F.subTo(this.m, F) } }
    function montSqrTo(C, D) { C.squareTo(D); this.reduce(D) }
    function montMulTo(D, E, F) { D.multiplyTo(E, F); this.reduce(F) }
    Montgomery.prototype.convert = montConvert;
    Montgomery.prototype.revert = montRevert;
    Montgomery.prototype.reduce = montReduce;
    Montgomery.prototype.mulTo = montMulTo;
    Montgomery.prototype.sqrTo = montSqrTo;
    function bnpIsEven() { return ((this.t > 0) ? (this[0] & 1) : this.s) == 0 }
    function bnpExp(M, K) { if (M > 4294967295 || M < 1) { return BigInteger.ONE } var L = nbi(), H = nbi(), N = K.convert(this), J = nbits(M) - 1; N.copyTo(L); while (--J >= 0) { K.sqrTo(L, H); if ((M & (1 << J)) > 0) { K.mulTo(H, N, L) } else { var I = L; L = H; H = I } } return K.revert(L) }
    function bnModPowInt(F, E) { var D; if (F < 256 || E.isEven()) { D = new Classic(E) } else { D = new Montgomery(E) } return this.exp(F, D) }

    function bnIntValue() { if (this.s < 0) { if (this.t == 1) { return this[0] - this.DV } else { if (this.t == 0) { return -1 } } } else { if (this.t == 1) { return this[0] } else { if (this.t == 0) { return 0 } } } return ((this[1] & ((1 << (32 - this.DB)) - 1)) << this.DB) | this[0] };
    function bnpChunkSize(r) { return Math.floor(Math.LN2 * this.DB / Math.log(r)); }
    function bnSigNum() { if (this.s < 0) { return -1 } else { if (this.t <= 0 || (this.t == 1 && this[0] <= 0)) { return 0 } else { return 1 } } };
    function bnpToRadix(b) { if (b == null) { b = 10 } if (this.signum() == 0 || b < 2 || b > 36) { return "0" } var cs = this.chunkSize(b); var a = Math.pow(b, cs); var d = nbv(a), y = nbi(), z = nbi(), r = ""; this.divRemTo(d, y, z); while (y.signum() > 0) { r = (a + z.intValue()).toString(b).substr(1) + r; y.divRemTo(d, y, z) } return z.intValue().toString(b) + r };
    function bnToByteArray() { var i = this.t, r = new Array(); r[0] = this.s; var p = this.DB - (i * this.DB) % 8, d, k = 0; if (i-- > 0) { if (p < this.DB && (d = this[i] >> p) != (this.s & this.DM) >> p) { r[k++] = d | (this.s << (this.DB - p)) } while (i >= 0) { if (p < 8) { d = (this[i] & ((1 << p) - 1)) << (8 - p); d |= this[--i] >> (p += this.DB - 8) } else { d = (this[i] >> (p -= 8)) & 255; if (p <= 0) { p += this.DB; --i } } if ((d & 128) != 0) { d |= -256 } if (k == 0 && (this.s & 128) != (d & 128)) { ++k } if (k > 0 || d != this.s) { r[k++] = d } } } return r };
    function bnpFromRadix(s, b) { this.fromInt(0); if (b == null) { b = 10 } var cs = this.chunkSize(b); var d = Math.pow(b, cs), mi = false, j = 0, w = 0; for (var i = 0; i < s.length; ++i) { var x = intAt(s, i); if (x < 0) { if (s.charAt(i) == "-" && this.signum() == 0) { mi = true } continue } w = b * w + x; if (++j >= cs) { this.dMultiply(d); this.dAddOffset(w, 0); j = 0; w = 0 } } if (j > 0) { this.dMultiply(Math.pow(b, j)); this.dAddOffset(w, 0) } if (mi) { BigInteger.ZERO.subTo(this, this) } };
    function bnpDMultiply(n) { this[this.t] = this.am(0, n - 1, this, 0, 0, this.t); ++this.t; this.clamp() };
    function bnpDAddOffset(n, w) { if (n == 0) return; while (this.t <= w) this[this.t++] = 0; this[w] += n; while (this[w] >= this.DV) { this[w] -= this.DV; if (++w >= this.t) this[this.t++] = 0; ++this[w] } }

    BigInteger.prototype.copyTo = bnpCopyTo;
    BigInteger.prototype.fromInt = bnpFromInt;
    BigInteger.prototype.fromString = bnpFromString;
    BigInteger.prototype.clamp = bnpClamp;
    BigInteger.prototype.dlShiftTo = bnpDLShiftTo;
    BigInteger.prototype.drShiftTo = bnpDRShiftTo;
    BigInteger.prototype.lShiftTo = bnpLShiftTo;
    BigInteger.prototype.rShiftTo = bnpRShiftTo;
    BigInteger.prototype.subTo = bnpSubTo;
    BigInteger.prototype.multiplyTo = bnpMultiplyTo;
    BigInteger.prototype.squareTo = bnpSquareTo;
    BigInteger.prototype.divRemTo = bnpDivRemTo;
    BigInteger.prototype.invDigit = bnpInvDigit;
    BigInteger.prototype.isEven = bnpIsEven;
    BigInteger.prototype.exp = bnpExp;
    BigInteger.prototype.toString = bnToString;
    BigInteger.prototype.negate = bnNegate;
    BigInteger.prototype.abs = bnAbs;
    BigInteger.prototype.compareTo = bnCompareTo;
    BigInteger.prototype.bitLength = bnBitLength;
    BigInteger.prototype.mod = bnMod;

    BigInteger.prototype.intValue = bnIntValue;
    BigInteger.prototype.chunkSize = bnpChunkSize;
    BigInteger.prototype.modPowInt = bnModPowInt;
    BigInteger.prototype.signum = bnSigNum;
    BigInteger.prototype.toRadix = bnpToRadix;
    BigInteger.prototype.toByteArray = bnToByteArray;
    BigInteger.prototype.fromRadix = bnpFromRadix;
    BigInteger.prototype.dMultiply = bnpDMultiply;
    BigInteger.prototype.dAddOffset = bnpDAddOffset;
    BigInteger.ZERO = nbv(0);
    BigInteger.ONE = nbv(1);
    function Arcfour() { this.i = 0; this.j = 0; this.S = new Array() }
    function ARC4init(H) { var F, E, G; for (F = 0; F < 256; ++F) { this.S[F] = F } E = 0; for (F = 0; F < 256; ++F) { E = (E + this.S[F] + H[F % H.length]) & 255; G = this.S[F]; this.S[F] = this.S[E]; this.S[E] = G } this.i = 0; this.j = 0 }
    function ARC4next() { var B; this.i = (this.i + 1) & 255; this.j = (this.j + this.S[this.i]) & 255; B = this.S[this.i]; this.S[this.i] = this.S[this.j]; this.S[this.j] = B; return this.S[(B + this.S[this.i]) & 255] }
    Arcfour.prototype.init = ARC4init;
    Arcfour.prototype.next = ARC4next;
    function prng_newstate() { return new Arcfour() }
    function rng_seed_int(B) { rng_pool[rng_pptr++] ^= B & 255; rng_pool[rng_pptr++] ^= (B >> 8) & 255; rng_pool[rng_pptr++] ^= (B >> 16) & 255; rng_pool[rng_pptr++] ^= (B >> 24) & 255; if (rng_pptr >= rng_psize) { rng_pptr -= rng_psize } }
    function rng_seed_time() { rng_seed_int(new Date().getTime()) } if (rng_pool == null) { rng_pool = new Array(); rng_pptr = 0; var t; if (window.crypto && window.crypto.getRandomValues) { var ua = new Uint8Array(32); window.crypto.getRandomValues(ua); for (t = 0; t < 32; ++t) { rng_pool[rng_pptr++] = ua[t] } } if (navigator.appName == "Netscape" && navigator.appVersion < "5" && window.crypto) { var z = window.crypto.random(32); for (t = 0; t < z.length; ++t) { rng_pool[rng_pptr++] = z.charCodeAt(t) & 255 } } while (rng_pptr < rng_psize) { t = Math.floor(65536 * Math.random()); rng_pool[rng_pptr++] = t >>> 8; rng_pool[rng_pptr++] = t & 255 } rng_pptr = 0; rng_seed_time() }
    function rng_get_byte() { if (rng_state == null) { rng_seed_time(); rng_state = prng_newstate(); rng_state.init(rng_pool); for (rng_pptr = 0; rng_pptr < rng_pool.length; ++rng_pptr) { rng_pool[rng_pptr] = 0 } rng_pptr = 0 } return rng_state.next() }
    function rng_get_bytes(C) { var D; for (D = 0; D < C.length; ++D) { C[D] = rng_get_byte() } }
    function SecureRandom() { }
    SecureRandom.prototype.nextBytes = rng_get_bytes;
    function parseBigInt(C, D) { return new BigInteger(C, D) }
    function pkcs1pad2(L, M) { if (M < L.length + 11) { alert("Message too long for RSA"); return null } var K = new Array(); var N = L.length - 1; while (N >= 0 && M > 0) { var I = L.charCodeAt(N--); if (I < 128) { K[--M] = I } else { if ((I > 127) && (I < 2048)) { K[--M] = (I & 63) | 128; K[--M] = (I >> 6) | 192 } else { K[--M] = (I & 63) | 128; K[--M] = ((I >> 6) & 63) | 128; K[--M] = (I >> 12) | 224 } } } K[--M] = 0; var H = new SecureRandom(); var J = new Array(); while (M > 2) { J[0] = 0; while (J[0] == 0) { H.nextBytes(J) } K[--M] = J[0] } K[--M] = 2; K[--M] = 0; return new BigInteger(K) }
    function RSAKey() { this.n = null; this.e = 0; this.d = null; this.p = null; this.q = null; this.dmp1 = null; this.dmq1 = null; this.coeff = null }
    function RSASetPublic(D, C) { if (D != null && C != null && D.length > 0 && C.length > 0) { this.n = parseBigInt(D, 16); this.e = parseInt(C, 16) } else { alert("Invalid RSA public key") } }
    function RSADoPublic(B) { return B.modPowInt(this.e, this.n) }
    function RSAEncrypt(F) { var G = pkcs1pad2(F, (this.n.bitLength() + 7) >> 3); if (G == null) { return null } var E = this.doPublic(G); if (E == null) { return null } var H = E.toString(16); if ((H.length & 1) == 0) { return H } else { return "0" + H } }
    RSAKey.prototype.doPublic = RSADoPublic;
    RSAKey.prototype.setPublic = RSASetPublic;
    RSAKey.prototype.encrypt = RSAEncrypt;
    function hex2b64(A) { var B; var D; var C = ""; for (B = 0; B + 3 <= A.length; B += 3) { D = parseInt(A.substring(B, B + 3), 16); C += b64map.charAt(D >> 6) + b64map.charAt(D & 63) } if (B + 1 == A.length) { D = parseInt(A.substring(B, B + 1), 16); C += b64map.charAt(D << 2) } else { if (B + 2 == A.length) { D = parseInt(A.substring(B, B + 2), 16); C += b64map.charAt(D >> 2) + b64map.charAt((D & 3) << 4) } } while ((C.length & 3) > 0) { C += b64padchar } return C }
    function bigintToText(a) { for (var e, b = a.toByteArray(), c = -1, d = ""; ++c < b.length;)e = 255 & b[c], 128 > e ? d += String.fromCharCode(e) : e > 191 && 224 > e ? (d += String.fromCharCode((31 & e) << 6 | 63 & b[c + 1]), ++c) : (d += String.fromCharCode((15 & e) << 12 | (63 & b[c + 1]) << 6 | 63 & b[c + 2]), c += 2); return d }
    function RSADecrypt2(a, b, c) { var d = parseBigInt(b, 16), e = parseBigInt(c, 16), f = parseBigInt(a, 16), g = f.modPowInt(e, d); return null == g ? null : bigintToText(g) }
    RSAKey.prototype.decrypt2 = RSADecrypt2;

    function rsa(ctext, N, E) { var D = new RSAKey(); D.setPublic(N, E); var res = D.encrypt(ctext); return hex2b64(res) };
    function rsaD(ctext, N, E) { var r = new RSAKey(); return r.decrypt2(ctext, N, E); }
    function rcx(data, pass) { function GetKey(pass, klen) { var mBox = []; for (var i = 0; i < klen; i++) { mBox.push(i) } var j = 0; for (var i = 0; i < klen; i++) { var p = pass.charCodeAt(i % pass.length); j = (j + mBox[i] + p) % klen; var temp = mBox[i]; mBox[i] = mBox[j]; mBox[j] = temp } return mBox } var mBox = GetKey(pass, 256); var bytes = data; if (typeof data === "string") { bytes = stringToByte(data) } var output = []; var i = 0, j = 0; for (var offset = 0; offset < bytes.length; offset++) { i = (++i) & 255; j = (j + mBox[i]) & 255; var a = bytes[offset]; var c = (a ^ mBox[(mBox[i] + mBox[j]) & 255]); output.push(c); var temp2 = mBox[c]; mBox[c] = mBox[a]; mBox[a] = temp2; j = (j + a + c) } return output };
    //function threeRcx(data, pass) { var output = rcx(data, pass); output.reverse(); output = rcx(output, pass); output.reverse(); output = rcx(output, pass); return output };
    //function stringToByte(str) { var bytes = new Array(); var len, c; len = str.length; for (var i = 0; i < len; i++) { c = str.charCodeAt(i); if (c >= 65536 && c <= 1114111) { bytes.push(((c >> 18) & 7) | 240); bytes.push(((c >> 12) & 63) | 128); bytes.push(((c >> 6) & 63) | 128); bytes.push((c & 63) | 128) } else { if (c >= 2048 && c <= 65535) { bytes.push(((c >> 12) & 15) | 224); bytes.push(((c >> 6) & 63) | 128); bytes.push((c & 63) | 128) } else { if (c >= 128 && c <= 2047) { bytes.push(((c >> 6) & 31) | 192); bytes.push((c & 63) | 128) } else { bytes.push(c & 255) } } } } return bytes };
    function arrayToBase64(data) { var out, i, len; var c1, c2, c3; len = data.length; i = 0; out = ""; while (i < len) { c1 = data[i++] & 255; if (i == len) { out += b64map.charAt(c1 >> 2); out += b64map.charAt((c1 & 3) << 4); out += "=="; break } c2 = data[i++]; if (i == len) { out += b64map.charAt(c1 >> 2); out += b64map.charAt(((c1 & 3) << 4) | ((c2 & 240) >> 4)); out += b64map.charAt((c2 & 15) << 2); out += "="; break } c3 = data[i++]; out += b64map.charAt(c1 >> 2); out += b64map.charAt(((c1 & 3) << 4) | ((c2 & 240) >> 4)); out += b64map.charAt(((c2 & 15) << 2) | ((c3 & 192) >> 6)); out += b64map.charAt(c3 & 63) } return out };
    function base64ToArray(str) { var base64DecodeChars = new Array(-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 62, -1, -1, -1, 63, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, -1, -1, -1, -1, -1, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, -1, -1, -1, -1, -1, -1, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, -1, -1, -1, -1, -1); var c1, c2, c3, c4; var len = str.length; var i = 0; var out = []; while (i < len) { do { c1 = base64DecodeChars[str.charCodeAt(i++) & 255] } while (i < len && c1 == -1); if (c1 == -1) { break } do { c2 = base64DecodeChars[str.charCodeAt(i++) & 255] } while (i < len && c2 == -1); if (c2 == -1) { break } out.push((c1 << 2) | ((c2 & 48) >> 4)); do { c3 = str.charCodeAt(i++) & 255; if (c3 == 61) { return out } c3 = base64DecodeChars[c3] } while (i < len && c3 == -1); if (c3 == -1) { break } out.push(((c2 & 15) << 4) | ((c3 & 60) >> 2)); do { c4 = str.charCodeAt(i++) & 255; if (c4 == 61) { return out } c4 = base64DecodeChars[c4] } while (i < len && c4 == -1); if (c4 == -1) { break } out.push(((c3 & 3) << 6) | c4) } return out };
    function stringToByte(s) {
        const utf8 = [];
        for (var ii = 0; ii < s.length; ii++) {
            var charCode = s.charCodeAt(ii);
            if (charCode < 0x80) utf8.push(charCode);
            else if (charCode < 0x800) {
                utf8.push(0xc0 | (charCode >> 6), 0x80 | (charCode & 0x3f));
            } else if (charCode < 0xd800 || charCode >= 0xe000) {
                utf8.push(0xe0 | (charCode >> 12), 0x80 | ((charCode >> 6) & 0x3f), 0x80 | (charCode & 0x3f));
            } else {
                ii++;
                charCode = 0x10000 + (((charCode & 0x3ff) << 10) | (s.charCodeAt(ii) & 0x3ff));
                utf8.push(
                    0xf0 | (charCode >> 18),
                    0x80 | ((charCode >> 12) & 0x3f),
                    0x80 | ((charCode >> 6) & 0x3f),
                    0x80 | (charCode & 0x3f)
                );
            }
        }
        return utf8;
    };

    function byteToString(bytes) {
        var str = "";
        for (var pos = 0; pos < bytes.length;) {
            var flag = bytes[pos];
            var unicode = 0;
            if ((flag >>> 7) === 0) {
                str += String.fromCharCode(bytes[pos]);
                pos += 1;
            //} else if ((flag & 0xFC) === 0xFC) {
            //    unicode = (bytes[pos] & 0x3) << 30;
            //    unicode |= (bytes[pos + 1] & 0x3F) << 24;
            //    unicode |= (bytes[pos + 2] & 0x3F) << 18;
            //    unicode |= (bytes[pos + 3] & 0x3F) << 12;
            //    unicode |= (bytes[pos + 4] & 0x3F) << 6;
            //    unicode |= (bytes[pos + 5] & 0x3F);
            //    str += String.fromCodePoint(unicode);
            //    pos += 6;
            //} else if ((flag & 0xF8) === 0xF8) {
            //    unicode = (bytes[pos] & 0x7) << 24;
            //    unicode |= (bytes[pos + 1] & 0x3F) << 18;
            //    unicode |= (bytes[pos + 2] & 0x3F) << 12;
            //    unicode |= (bytes[pos + 3] & 0x3F) << 6;
            //    unicode |= (bytes[pos + 4] & 0x3F);
            //    str += String.fromCodePoint(unicode);
            //    pos += 5;
            } else if ((flag & 0xF0) === 0xF0) {
                unicode = (bytes[pos] & 0xF) << 18;
                unicode |= (bytes[pos + 1] & 0x3F) << 12;
                unicode |= (bytes[pos + 2] & 0x3F) << 6;
                unicode |= (bytes[pos + 3] & 0x3F);
                str += String.fromCodePoint(unicode);
                pos += 4;
            } else if ((flag & 0xE0) === 0xE0) {
                unicode = (bytes[pos] & 0x1F) << 12;;
                unicode |= (bytes[pos + 1] & 0x3F) << 6;
                unicode |= (bytes[pos + 2] & 0x3F);
                str += String.fromCharCode(unicode);
                pos += 3;
            } else if ((flag & 0xC0) === 0xC0) {
                unicode = (bytes[pos] & 0x3F) << 6;
                unicode |= (bytes[pos + 1] & 0x3F);
                str += String.fromCharCode(unicode);
                pos += 2;
            } else {
                str += String.fromCharCode(bytes[pos]);
                pos += 1;
            }
        }
        return str;
    }

    setTimeout(function () {
        if (location.pathname.toLowerCase() == "/sqls") {
            if (window.addMenuTab) {
                addMenuTab("主页", "https://sqlonline.toolgood.com/")
            }
        }
        if (location.pathname.toLowerCase() == "/developments") {
            if (window.addMenuTab) {
                addMenuTab("主页", "https://sqlonline.toolgood.com/")
            }
        }
    }, 500);

    var _modulus = null;
    var _exponent = null;
    var _key = null;
    var _timeDiff = null;
    function init(modulus, exponent) {
        if (modulus) {
            _modulus = modulus;
            a = arrayToBase64(rcx(stringToByte(modulus), "9638521470"));
            localStorage.setItem("a", a);

            _exponent = exponent;
            b = arrayToBase64(rcx(stringToByte(exponent), "0987654321"));
            localStorage.setItem("b", b);

            _key = guid();
            localStorage.removeItem("c");

            var oldtime = new Date().valueOf();
            $.get("/Ajax/GetCurrTime", { st: oldtime }, function (data) {
                if (data.state == "SUCCESS") {
                    _timeDiff = parseInt(data.data);
                    localStorage.setItem("d", _timeDiff);
                }
            })
        } else if (_modulus == null) {
            a = localStorage.getItem("a");
            _modulus = byteToString(rcx(base64ToArray(a), "9638521470"));

            b = localStorage.getItem("b");
            _exponent = byteToString(rcx(base64ToArray(b), "0987654321"));

            var key = md5(_modulus + "|" + _exponent);
            var c = localStorage.getItem("c");
            var bytes = base64ToArray(c);
            bytes = rcx(bytes, key);
            var s = "";
            for (var i = 0; i < bytes.length; i++) {
                if (bytes[i] < 16) { s += "0"; }
                s += bytes[i].toString(16);
            }
            text = s.toUpperCase();
            _key = rsaD(text, _modulus, _exponent);

            var d = localStorage.getItem("d");
            _timeDiff = parseInt(d);
        }
    }

    function login(url, data, callback, errCallback) {
        init();
        var timestamp = new Date().valueOf() + _timeDiff;
        var key = _key;
        var ciphertext = '';
        if (data != undefined) {
            ciphertext = arrayToBase64(rcx(stringToByte(JSON.stringify(data)), key));
        }
        var rsaKey = rsa(_key, _modulus, _exponent);
        var sign = md5(ciphertext + "|" + _key + "|" + timestamp + "|" + _modulus + "|" + _exponent);
        var pastData = { rsaKey: rsaKey, ciphertext: ciphertext, timestamp: timestamp, sign: sign };

        var headers = {};
        var AntiforgeryToken = data["__RequestVerificationToken"];
        if (AntiforgeryToken != undefined) {
            headers["__RequestVerificationToken"] = AntiforgeryToken;
        }

        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json;charset=UTF-8",
            data: JSON.stringify(pastData),
            dataType: "JSON",
            headers: headers,
            success: function (data, textStatus, jqXHR) {
                if (callback != undefined) {
                    if (data.ciphertext != undefined && data.ciphertext.length > 2) {
                        var bytes = base64ToArray(data.ciphertext);
                        var json = byteToString(rcx(bytes, key));
                        data.data = JSON.parse(json);
                        if (localStorage && localStorage.getItem("debug")) {
                            console.log("[" + url + "]text: " + json);
                        }
                    }
                    if (data.data && data.data.rsaKey) {
                        var key = md5(_modulus + "|" + _exponent);
                        c = arrayToBase64(rcx(base64ToArray(data.data.rsaKey), key));
                        localStorage.setItem("c", c);
                    }
                    callback(data, textStatus, jqXHR);
                }
            },
            error: function (data, textStatus, errorThrown) { if (errCallback != undefined) { errCallback(data, textStatus, errorThrown); } }
        });
    }

    function rsaPost(url, data, appendData, callback, errCallback) {
        if (jQuery.isFunction(data)) { errCallback = errCallback || appendData; callback = data; data = undefined; appendData = undefined; }
        else if (jQuery.isFunction(appendData)) { errCallback = errCallback || callback; callback = appendData; appendData = undefined; }

        init();
        var timestamp = new Date().valueOf() + _timeDiff;
        var key = _key;
        var ciphertext = '';
        if (data != undefined) {
            ciphertext = arrayToBase64(rcx(stringToByte(JSON.stringify(data)), key));
        }
        var sign = md5(ciphertext + "|" + _key + "|" + timestamp + "|" + _modulus + "|" + _exponent);

        var post = { ciphertext: ciphertext, timestamp: timestamp, sign: sign };
        if (appendData != undefined) {
            for (var key in appendData) {
                if (appendData[key] != undefined) { post[key] = appendData[key]; }
            }
        }

        var headers = {};
        var AntiforgeryToken = data["__RequestVerificationToken"];
        if (AntiforgeryToken != undefined) {
            /*post["__RequestVerificationToken"] = AntiforgeryToken;*/
            headers["__RequestVerificationToken"] = AntiforgeryToken;
        }

        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json;charset=UTF-8",
            data: JSON.stringify(post),
            dataType: "JSON",
            headers: headers,
            success: function (data, textStatus, jqXHR) {
                if (callback != undefined) {
                    if (data.ciphertext != undefined && data.ciphertext.length > 2) {
                        var bytes = base64ToArray(data.ciphertext);
                        var json = byteToString(rcx(bytes, key));
                        data.data = JSON.parse(json);
                        if (localStorage && localStorage.getItem("debug")) {
                            console.log("[" + url + "]text: " + json);
                        }
                    }
                    callback(data, textStatus, jqXHR);
                }
            },
            error: function (data, textStatus, errorThrown) { if (errCallback != undefined) { errCallback(data, textStatus, errorThrown); } }
        });
    }

    function adminModePost(url, data, tarUrlOrFunction, errCallback) {
        var oldData = data;
        rsaPost(url, data, function (data, textStatus, jqXHR) {
            if (data.state == "SUCCESS") {
                if (typeof tarUrlOrFunction == 'function') {
                    tarUrlOrFunction(data, textStatus, jqXHR);
                } else {
                    location.href = tarUrlOrFunction;
                }
            } else if (data.message == "TryAdminMode") {
                layer.open({
                    type: 2,
                    title: "需要确认是您本人操作",
                    content: "/admins/tools/AdminModeWindow",
                    btn: ['确定', '取消'],
                    area: ['650px', '330px'],
                    yes: function (index, layero) {
                        var AdminModeTime = layer.getChildFrame('select[name=AdminModeTime]', index).val();
                        var OperatorPassword = layer.getChildFrame('input#OperatorPassword', index).val();
                        layer.getChildFrame('input[name=OperatorPassword]', index).val(OperatorPassword);

                        var str = "";
                        for (var i = 0; i < OperatorPassword.length; i++) { str += "*"; }
                        layer.getChildFrame('input#OperatorPassword', index).val(str);

                        if (OperatorPassword.length <= 4) {
                            layer.alert("请输入密码！");
                        } else {
                            oldData.OperatorPassword = OperatorPassword;
                            oldData.AdminModeTime = AdminModeTime;
                            adminModePost(url, oldData, function (data, textStatus, jqXHR) {
                                layer.close(index);
                                if (typeof tarUrlOrFunction == 'function') {
                                    tarUrlOrFunction(data, textStatus, jqXHR);
                                } else {
                                    location.href = tarUrlOrFunction;
                                }
                            }, function (data, textStatus, errorThrown) {
                                layer.getChildFrame('input#OperatorPassword', index).val(OperatorPassword);
                                data.message && layer.alert(data.message);
                            });
                        }
                    }
                });
            } else {
                if (errCallback != undefined) {
                    errCallback(data, textStatus, null, jqXHR);
                } else {
                    layer.alert(data.message);
                }
            }
        }, errCallback);
    }

    $.init = init;
    $.rsaPost = rsaPost;
    $.adminModePost = adminModePost;
    $.login = login;

    $.encrypt = function (data) {
        init();
        var timestamp = new Date().valueOf() + _timeDiff;
        var ciphertext = '';
        if (data) {
            ciphertext = arrayToBase64(rcx(stringToByte(JSON.stringify(data)), _key));
        } else {
            ciphertext = arrayToBase64(rcx(stringToByte("{}"), _key));
        }
        var sign = md5(ciphertext + "|" + _key + "|" + timestamp + "|" + _modulus + "|" + _exponent);
        return { ciphertext: ciphertext, timestamp: timestamp, sign: sign };
    }
    $.decrypt = function (data, url) {
        init();
        if (data == undefined) { return undefined; }
        if (data == null) { return null; }
        var json = byteToString(rcx(base64ToArray(data), _key));

        if (localStorage && localStorage.getItem("debug")) {
            console.log("[" + url + "]Text: " + json);
        }
        return json;
    }
    $.downLoadFile = function (options) {
        var config = $.extend(true, { method: 'post' }, options);
        var tmpParam = config.data;
        config.data = $.encrypt(tmpParam);

        var $iframe = $('<iframe id="down-file-iframe" />');
        var $form = $('<form target="down-file-iframe" method="' + config.method + '" />');
        $form.attr('action', config.url);

        for (var key in config.data) {
            $form.append('<input type="hidden" name="' + key + '" value="' + config.data[key] + '" />');
        }
        $iframe.append($form);
        $(document.body).append($iframe);
        $form[0].submit();
        $iframe.remove();
    }
}));