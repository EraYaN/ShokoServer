section .text

global _SHA_Compile_p5@0
align 10h

_SHA_Compile_p5@0:
   mov     edi, [ebp+0]
   bswap   edi
   mov     [esp+4h], edi
   mov     esi, [ebp+4]
   bswap   esi
   mov     [esp+8h], esi
   mov     ebx, [ebp+8]
   bswap   ebx
   mov     [esp+0Ch], ebx
   mov     ebx, [ebp+0Ch]
   bswap   ebx
   mov     [esp+10h], ebx
   mov     ebx, [ebp+10h]
   bswap   ebx
   mov     [esp+14h], ebx
   mov     ebx, [ebp+14h]
   bswap   ebx
   mov     [esp+18h], ebx
   mov     ebx, [ebp+18h]
   bswap   ebx
   mov     [esp+1Ch], ebx
   mov     ebx, [ebp+1Ch]
   bswap   ebx
   mov     [esp+20h], ebx
   mov     ebx, [ebp+20h]
   bswap   ebx
   mov     [esp+24h], ebx
   mov     ebx, [ebp+24h]
   bswap   ebx
   mov     [esp+28h], ebx
   mov     ebx, [ebp+28h]
   bswap   ebx
   mov     [esp+2Ch], ebx
   mov     ebx, [ebp+2Ch]
   bswap   ebx
   mov     [esp+30h], ebx
   mov     ebx, [ebp+30h]
   bswap   ebx
   mov     [esp+34h], ebx
   mov     edx, [ebp+34h]
   bswap   edx
   mov     [esp+38h], edx
   mov     ecx, [ebp+38h]
   bswap   ecx
   mov     [esp+3Ch], ecx
   mov     ebx, [ebp+3Ch]
   bswap   ebx
   mov     [esp+40h], ebx
   xor     edx, edi
   mov     edi, [esp+0Ch]
   xor     edx, edi
   xor     edx, [esp+24h]
   rol     edx, 1
   mov     [esp+44h], edx
   xor     ecx, esi
   mov     esi, [esp+10h]
   xor     ecx, esi
   xor     ecx, [esp+28h]
   rol     ecx, 1
   mov     [esp+48h], ecx
   xor     ebx, edi
   mov     edi, [esp+14h]
   xor     ebx, edi
   xor     ebx, [esp+2Ch]
   rol     ebx, 1
   mov     [esp+4Ch], ebx
   xor     edx, esi
   mov     esi, [esp+18h]
   xor     edx, esi
   xor     edx, [esp+30h]
   rol     edx, 1
   mov     [esp+50h], edx
   xor     ecx, edi
   mov     edi, [esp+1Ch]
   xor     ecx, edi
   xor     ecx, [esp+34h]
   rol     ecx, 1
   mov     [esp+54h], ecx
   xor     ebx, esi
   mov     esi, [esp+20h]
   xor     ebx, esi
   xor     ebx, [esp+38h]
   rol     ebx, 1
   mov     [esp+58h], ebx
   xor     edx, edi
   mov     edi, [esp+24h]
   xor     edx, edi
   xor     edx, [esp+3Ch]
   rol     edx, 1
   mov     [esp+5Ch], edx
   xor     ecx, esi
   mov     esi, [esp+28h]
   xor     ecx, esi
   xor     ecx, [esp+40h]
   rol     ecx, 1
   mov     [esp+60h], ecx
   xor     ebx, edi
   mov     edi, [esp+2Ch]
   xor     ebx, edi
   xor     ebx, [esp+44h]
   rol     ebx, 1
   mov     [esp+64h], ebx
   xor     edx, esi
   mov     esi, [esp+30h]
   xor     edx, esi
   xor     edx, [esp+48h]
   rol     edx, 1
   mov     [esp+68h], edx
   xor     ecx, edi
   mov     edi, [esp+34h]
   xor     ecx, edi
   xor     ecx, [esp+4Ch]
   rol     ecx, 1
   mov     [esp+6Ch], ecx
   xor     ebx, esi
   mov     esi, [esp+38h]
   xor     ebx, esi
   xor     ebx, [esp+50h]
   rol     ebx, 1
   mov     [esp+70h], ebx
   xor     edx, edi
   mov     edi, [esp+3Ch]
   xor     edx, edi
   xor     edx, [esp+54h]
   rol     edx, 1
   mov     [esp+74h], edx
   xor     ecx, esi
   mov     esi, [esp+40h]
   xor     ecx, esi
   xor     ecx, [esp+58h]
   rol     ecx, 1
   mov     [esp+78h], ecx
   xor     ebx, edi
   mov     edi, [esp+44h]
   xor     ebx, edi
   xor     ebx, [esp+5Ch]
   rol     ebx, 1
   mov     [esp+7Ch], ebx
   xor     edx, esi
   mov     esi, [esp+48h]
   xor     edx, esi
   xor     edx, [esp+60h]
   rol     edx, 1
   mov     [esp+80h], edx
   xor     ecx, edi
   mov     edi, [esp+4Ch]
   xor     ecx, edi
   xor     ecx, [esp+64h]
   rol     ecx, 1
   mov     [esp+84h], ecx
   xor     ebx, esi
   mov     esi, [esp+50h]
   xor     ebx, esi
   xor     ebx, [esp+68h]
   rol     ebx, 1
   mov     [esp+88h], ebx
   xor     edx, edi
   mov     edi, [esp+54h]
   xor     edx, edi
   xor     edx, [esp+6Ch]
   rol     edx, 1
   mov     [esp+8Ch], edx
   xor     ecx, esi
   mov     esi, [esp+58h]
   xor     ecx, esi
   xor     ecx, [esp+70h]
   rol     ecx, 1
   mov     [esp+90h], ecx
   xor     ebx, edi
   mov     edi, [esp+5Ch]
   xor     ebx, edi
   xor     ebx, [esp+74h]
   rol     ebx, 1
   mov     [esp+94h], ebx
   xor     edx, esi
   mov     esi, [esp+60h]
   xor     edx, esi
   xor     edx, [esp+78h]
   rol     edx, 1
   mov     [esp+98h], edx
   xor     ecx, edi
   mov     edi, [esp+64h]
   xor     ecx, edi
   xor     ecx, [esp+7Ch]
   rol     ecx, 1
   mov     [esp+9Ch], ecx
   xor     ebx, esi
   mov     esi, [esp+68h]
   xor     ebx, esi
   xor     ebx, [esp+80h]
   rol     ebx, 1
   mov     [esp+0A0h], ebx
   xor     edx, edi
   mov     edi, [esp+6Ch]
   xor     edx, edi
   xor     edx, [esp+84h]
   rol     edx, 1
   mov     [esp+0A4h], edx
   xor     ecx, esi
   mov     esi, [esp+70h]
   xor     ecx, esi
   xor     ecx, [esp+88h]
   rol     ecx, 1
   mov     [esp+0A8h], ecx
   xor     ebx, edi
   mov     edi, [esp+74h]
   xor     ebx, edi
   xor     ebx, [esp+8Ch]
   rol     ebx, 1
   mov     [esp+0ACh], ebx
   xor     edx, esi
   mov     esi, [esp+78h]
   xor     edx, esi
   xor     edx, [esp+90h]
   rol     edx, 1
   mov     [esp+0B0h], edx
   xor     ecx, edi
   mov     edi, [esp+7Ch]
   xor     ecx, edi
   xor     ecx, [esp+94h]
   rol     ecx, 1
   mov     [esp+0B4h], ecx
   xor     ebx, esi
   mov     esi, [esp+80h]
   xor     ebx, esi
   xor     ebx, [esp+98h]
   rol     ebx, 1
   mov     [esp+0B8h], ebx
   xor     edx, edi
   mov     edi, [esp+84h]
   xor     edx, edi
   xor     edx, [esp+9Ch]
   rol     edx, 1
   mov     [esp+0BCh], edx
   xor     ecx, esi
   mov     esi, [esp+88h]
   xor     ecx, esi
   xor     ecx, [esp+0A0h]
   rol     ecx, 1
   mov     [esp+0C0h], ecx
   xor     ebx, edi
   mov     edi, [esp+8Ch]
   xor     ebx, edi
   xor     ebx, [esp+0A4h]
   rol     ebx, 1
   mov     [esp+0C4h], ebx
   xor     edx, esi
   mov     esi, [esp+90h]
   xor     edx, esi
   xor     edx, [esp+0A8h]
   rol     edx, 1
   mov     [esp+0C8h], edx
   xor     ecx, edi
   mov     edi, [esp+94h]
   xor     ecx, edi
   xor     ecx, [esp+0ACh]
   rol     ecx, 1
   mov     [esp+0CCh], ecx
   xor     ebx, esi
   mov     esi, [esp+98h]
   xor     ebx, esi
   xor     ebx, [esp+0B0h]
   rol     ebx, 1
   mov     [esp+0D0h], ebx
   xor     edx, edi
   mov     edi, [esp+9Ch]
   xor     edx, edi
   xor     edx, [esp+0B4h]
   rol     edx, 1
   mov     [esp+0D4h], edx
   xor     ecx, esi
   mov     esi, [esp+0A0h]
   xor     ecx, esi
   xor     ecx, [esp+0B8h]
   rol     ecx, 1
   mov     [esp+0D8h], ecx
   xor     ebx, edi
   mov     edi, [esp+0A4h]
   xor     ebx, edi
   xor     ebx, [esp+0BCh]
   rol     ebx, 1
   mov     [esp+0DCh], ebx
   xor     edx, esi
   mov     esi, [esp+0A8h]
   xor     edx, esi
   xor     edx, [esp+0C0h]
   rol     edx, 1
   mov     [esp+0E0h], edx
   xor     ecx, edi
   mov     edi, [esp+0ACh]
   xor     ecx, edi
   xor     ecx, [esp+0C4h]
   rol     ecx, 1
   mov     [esp+0E4h], ecx
   xor     ebx, esi
   mov     esi, [esp+0B0h]
   xor     ebx, esi
   xor     ebx, [esp+0C8h]
   rol     ebx, 1
   mov     [esp+0E8h], ebx
   xor     edx, edi
   mov     edi, [esp+0B4h]
   xor     edx, edi
   xor     edx, [esp+0CCh]
   rol     edx, 1
   mov     [esp+0ECh], edx
   xor     ecx, esi
   mov     esi, [esp+0B8h]
   xor     ecx, esi
   xor     ecx, [esp+0D0h]
   rol     ecx, 1
   mov     [esp+0F0h], ecx
   xor     ebx, edi
   mov     edi, [esp+0BCh]
   xor     ebx, edi
   xor     ebx, [esp+0D4h]
   rol     ebx, 1
   mov     [esp+0F4h], ebx
   xor     edx, esi
   mov     esi, [esp+0C0h]
   xor     edx, esi
   xor     edx, [esp+0D8h]
   rol     edx, 1
   mov     [esp+0F8h], edx
   xor     ecx, edi
   mov     edi, [esp+0C4h]
   xor     ecx, edi
   xor     ecx, [esp+0DCh]
   rol     ecx, 1
   mov     [esp+0FCh], ecx
   xor     ebx, esi
   mov     esi, [esp+0C8h]
   xor     ebx, esi
   xor     ebx, [esp+0E0h]
   rol     ebx, 1
   mov     [esp+100h], ebx
   xor     edx, edi
   mov     edi, [esp+0CCh]
   xor     edx, edi
   xor     edx, [esp+0E4h]
   rol     edx, 1
   mov     [esp+104h], edx
   xor     ecx, esi
   mov     esi, [esp+0D0h]
   xor     ecx, esi
   xor     ecx, [esp+0E8h]
   rol     ecx, 1
   mov     [esp+108h], ecx
   xor     ebx, edi
   mov     edi, [esp+0D4h]
   xor     ebx, edi
   xor     ebx, [esp+0ECh]
   rol     ebx, 1
   mov     [esp+10Ch], ebx
   xor     edx, esi
   mov     esi, [esp+0D8h]
   xor     edx, esi
   xor     edx, [esp+0F0h]
   rol     edx, 1
   mov     [esp+110h], edx
   xor     ecx, edi
   mov     edi, [esp+0DCh]
   xor     ecx, edi
   xor     ecx, [esp+0F4h]
   rol     ecx, 1
   mov     [esp+114h], ecx
   xor     ebx, esi
   mov     esi, [esp+0E0h]
   xor     ebx, esi
   xor     ebx, [esp+0F8h]
   rol     ebx, 1
   mov     [esp+118h], ebx
   xor     edx, edi
   mov     edi, [esp+0E4h]
   xor     edx, edi
   xor     edx, [esp+0FCh]
   rol     edx, 1
   mov     [esp+11Ch], edx
   xor     ecx, esi
   mov     esi, [esp+0E8h]
   xor     ecx, esi
   xor     ecx, [esp+100h]
   rol     ecx, 1
   mov     [esp+120h], ecx
   xor     ebx, edi
   mov     edi, [esp+0ECh]
   xor     ebx, edi
   xor     ebx, [esp+104h]
   rol     ebx, 1
   mov     [esp+124h], ebx
   xor     edx, esi
   mov     esi, [esp+0F0h]
   xor     edx, esi
   xor     edx, [esp+0108h]
   rol     edx, 1
   mov     [esp+128h], edx
   xor     ecx, edi
   mov     edi, [esp+0F4h]
   xor     ecx, edi
   xor     ecx, [esp+10Ch]
   rol     ecx, 1
   mov     [esp+12Ch], ecx
   xor     ebx, esi
   mov     esi, [esp+0F8h]
   xor     ebx, esi
   xor     ebx, [esp+110h]
   rol     ebx, 1
   mov     [esp+130h], ebx
   xor     edx, edi
   mov     edi, [esp+0FCh]
   xor     edx, edi
   xor     edx, [esp+114h]
   rol     edx, 1
   mov     [esp+134h], edx
   xor     ecx, esi
   mov     esi, [esp+100h]
   xor     ecx, esi
   xor     ecx, [esp+118h]
   rol     ecx, 1
   mov     [esp+138h], ecx
   xor     ebx, edi
   xor     ebx, [esp+104h]
   xor     ebx, [esp+11Ch]
   rol     ebx, 1
   mov     [esp+13Ch], ebx
   xor     edx, esi
   xor     edx, [esp+108h]
   xor     edx, [esp+120h]
   rol     edx, 1
   mov     [esp+140h], edx
   mov     ebp, [esp+168h]
   mov     eax, [ebp+8]
   mov     ebx, [ebp+0Ch]
   mov     ecx, [ebp+10h]
   mov     edx, [ebp+14h]
   mov     esi, [ebp+18h]
   mov     edi, eax
   mov     ebp, ecx
   rol     eax, 5
   xor     ebp, edx
   add     eax, esi
   and     ebp, ebx
   add     eax, 5A827999h
   xor     ebp, edx
   add     eax, [esp+4h]
   ror     ebx, 2
   add     eax, ebp
   mov     esi, eax
   mov     ebp, ebx
   rol     eax, 5
   xor     ebp, ecx
   add     eax, edx
   and     ebp, edi
   add     eax, 5A827999h
   xor     ebp, ecx
   add     eax, [esp+8h]
   ror     edi, 2
   add     eax, ebp
   mov     edx, eax
   mov     ebp, edi
   rol     eax, 5
   xor     ebp, ebx
   add     eax, ecx
   and     ebp, esi
   add     eax, 5A827999h
   xor     ebp, ebx
   add     eax, [esp+0Ch]
   ror     esi, 2
   add     eax, ebp
   mov     ecx, eax
   mov     ebp, esi
   rol     eax, 5
   xor     ebp, edi
   add     eax, ebx
   and     ebp, edx
   add     eax, 5A827999h
   xor     ebp, edi
   add     eax, [esp+10h]
   ror     edx, 2
   add     eax, ebp
   mov     ebx, eax
   mov     ebp, edx
   rol     eax, 5
   xor     ebp, esi
   add     eax, edi
   and     ebp, ecx
   add     eax, 5A827999h
   xor     ebp, esi
   add     eax, [esp+14h]
   ror     ecx, 2
   add     eax, ebp
   mov     edi, eax
   mov     ebp, ecx
   rol     eax, 5
   xor     ebp, edx
   add     eax, esi
   and     ebp, ebx
   add     eax, 5A827999h
   xor     ebp, edx
   add     eax, [esp+18h]
   ror     ebx, 2
   add     eax, ebp
   mov     esi, eax
   mov     ebp, ebx
   rol     eax, 5
   xor     ebp, ecx
   add     eax, edx
   and     ebp, edi
   add     eax, 5A827999h
   xor     ebp, ecx
   add     eax, [esp+1Ch]
   ror     edi, 2
   add     eax, ebp
   mov     edx, eax
   mov     ebp, edi
   rol     eax, 5
   xor     ebp, ebx
   add     eax, ecx
   and     ebp, esi
   add     eax, 5A827999h
   xor     ebp, ebx
   add     eax, [esp+20h]
   ror     esi, 2
   add     eax, ebp
   mov     ecx, eax
   mov     ebp, esi
   rol     eax, 5
   xor     ebp, edi
   add     eax, ebx
   and     ebp, edx
   add     eax, 5A827999h
   xor     ebp, edi
   add     eax, [esp+24h]
   ror     edx, 2
   add     eax, ebp
   mov     ebx, eax
   mov     ebp, edx
   rol     eax, 5
   xor     ebp, esi
   add     eax, edi
   and     ebp, ecx
   add     eax, 5A827999h
   xor     ebp, esi
   add     eax, [esp+28h]
   ror     ecx, 2
   add     eax, ebp
   mov     edi, eax
   mov     ebp, ecx
   rol     eax, 5
   xor     ebp, edx
   add     eax, esi
   and     ebp, ebx
   add     eax, 5A827999h
   xor     ebp, edx
   add     eax, [esp+2Ch]
   ror     ebx, 2
   add     eax, ebp
   mov     esi, eax
   mov     ebp, ebx
   rol     eax, 5
   xor     ebp, ecx
   add     eax, edx
   and     ebp, edi
   add     eax, 5A827999h
   xor     ebp, ecx
   add     eax, [esp+30h]
   ror     edi, 2
   add     eax, ebp
   mov     edx, eax
   mov     ebp, edi
   rol     eax, 5
   xor     ebp, ebx
   add     eax, ecx
   and     ebp, esi
   add     eax, 5A827999h
   xor     ebp, ebx
   add     eax, [esp+34h]
   ror     esi, 2
   add     eax, ebp
   mov     ecx, eax
   mov     ebp, esi
   rol     eax, 5
   xor     ebp, edi
   add     eax, ebx
   and     ebp, edx
   add     eax, 5A827999h
   xor     ebp, edi
   add     eax, [esp+38h]
   ror     edx, 2
   add     eax, ebp
   mov     ebx, eax
   mov     ebp, edx
   rol     eax, 5
   xor     ebp, esi
   add     eax, edi
   and     ebp, ecx
   add     eax, 5A827999h
   xor     ebp, esi
   add     eax, [esp+3Ch]
   ror     ecx, 2
   add     eax, ebp
   mov     edi, eax
   mov     ebp, ecx
   rol     eax, 5
   xor     ebp, edx
   add     eax, esi
   and     ebp, ebx
   add     eax, 5A827999h
   xor     ebp, edx
   add     eax, [esp+40h]
   ror     ebx, 2
   add     eax, ebp
   mov     esi, eax
   mov     ebp, ebx
   rol     eax, 5
   xor     ebp, ecx
   add     eax, edx
   and     ebp, edi
   add     eax, 5A827999h
   xor     ebp, ecx
   add     eax, [esp+44h]
   ror     edi, 2
   add     eax, ebp
   mov     edx, eax
   mov     ebp, edi
   rol     eax, 5
   xor     ebp, ebx
   add     eax, ecx
   and     ebp, esi
   add     eax, 5A827999h
   xor     ebp, ebx
   add     eax, [esp+48h]
   ror     esi, 2
   add     eax, ebp
   mov     ecx, eax
   mov     ebp, esi
   rol     eax, 5
   xor     ebp, edi
   add     eax, ebx
   and     ebp, edx
   add     eax, 5A827999h
   xor     ebp, edi
   add     eax, [esp+4Ch]
   ror     edx, 2
   add     eax, ebp
   mov     ebx, eax
   mov     ebp, edx
   rol     eax, 5
   xor     ebp, esi
   add     eax, edi
   and     ebp, ecx
   add     eax, 5A827999h
   xor     ebp, esi
   add     eax, [esp+50h]
   ror     ecx, 2
   add     eax, ebp
   mov     edi, eax
   rol     eax, 5
   mov     ebp, edx
   add     eax, esi
   xor     ebp, ecx
   add     eax, 6ED9EBA1h
   xor     ebp, ebx
   add     eax, [esp+54h]
   ror     ebx, 2
   add     eax, ebp
   mov     esi, eax
   rol     eax, 5
   mov     ebp, ecx
   add     eax, edx
   xor     ebp, ebx
   add     eax, 6ED9EBA1h
   xor     ebp, edi
   add     eax, [esp+58h]
   ror     edi, 2
   add     eax, ebp
   mov     edx, eax
   rol     eax, 5
   mov     ebp, ebx
   add     eax, ecx
   xor     ebp, edi
   add     eax, 6ED9EBA1h
   xor     ebp, esi
   add     eax, [esp+5Ch]
   ror     esi, 2
   add     eax, ebp
   mov     ecx, eax
   rol     eax, 5
   mov     ebp, edi
   add     eax, ebx
   xor     ebp, esi
   add     eax, 6ED9EBA1h
   xor     ebp, edx
   add     eax, [esp+60h]
   ror     edx, 2
   add     eax, ebp
   mov     ebx, eax
   rol     eax, 5
   mov     ebp, esi
   add     eax, edi
   xor     ebp, edx
   add     eax, 6ED9EBA1h
   xor     ebp, ecx
   add     eax, [esp+64h]
   ror     ecx, 2
   add     eax, ebp
   mov     edi, eax
   rol     eax, 5
   mov     ebp, edx
   add     eax, esi
   xor     ebp, ecx
   add     eax, 6ED9EBA1h
   xor     ebp, ebx
   add     eax, [esp+68h]
   ror     ebx, 2
   add     eax, ebp
   mov     esi, eax
   rol     eax, 5
   mov     ebp, ecx
   add     eax, edx
   xor     ebp, ebx
   add     eax, 6ED9EBA1h
   xor     ebp, edi
   add     eax, [esp+6Ch]
   ror     edi, 2
   add     eax, ebp
   mov     edx, eax
   rol     eax, 5
   mov     ebp, ebx
   add     eax, ecx
   xor     ebp, edi
   add     eax, 6ED9EBA1h
   xor     ebp, esi
   add     eax, [esp+70h]
   ror     esi, 2
   add     eax, ebp
   mov     ecx, eax
   rol     eax, 5
   mov     ebp, edi
   add     eax, ebx
   xor     ebp, esi
   add     eax, 6ED9EBA1h
   xor     ebp, edx
   add     eax, [esp+74h]
   ror     edx, 2
   add     eax, ebp
   mov     ebx, eax
   rol     eax, 5
   mov     ebp, esi
   add     eax, edi
   xor     ebp, edx
   add     eax, 6ED9EBA1h
   xor     ebp, ecx
   add     eax, [esp+78h]
   ror     ecx, 2
   add     eax, ebp
   mov     edi, eax
   rol     eax, 5
   mov     ebp, edx
   add     eax, esi
   xor     ebp, ecx
   add     eax, 6ED9EBA1h
   xor     ebp, ebx
   add     eax, [esp+7Ch]
   ror     ebx, 2
   add     eax, ebp
   mov     esi, eax
   rol     eax, 5
   mov     ebp, ecx
   add     eax, edx
   xor     ebp, ebx
   add     eax, 6ED9EBA1h
   xor     ebp, edi
   add     eax, [esp+80h]
   ror     edi, 2
   add     eax, ebp
   mov     edx, eax
   rol     eax, 5
   mov     ebp, ebx
   add     eax, ecx
   xor     ebp, edi
   add     eax, 6ED9EBA1h
   xor     ebp, esi
   add     eax, [esp+84h]
   ror     esi, 2
   add     eax, ebp
   mov     ecx, eax
   rol     eax, 5
   mov     ebp, edi
   add     eax, ebx
   xor     ebp, esi
   add     eax, 6ED9EBA1h
   xor     ebp, edx
   add     eax, [esp+88h]
   ror     edx, 2
   add     eax, ebp
   mov     ebx, eax
   rol     eax, 5
   mov     ebp, esi
   add     eax, edi
   xor     ebp, edx
   add     eax, 6ED9EBA1h
   xor     ebp, ecx
   add     eax, [esp+8Ch]
   ror     ecx, 2
   add     eax, ebp
   mov     edi, eax
   rol     eax, 5
   mov     ebp, edx
   add     eax, esi
   xor     ebp, ecx
   add     eax, 6ED9EBA1h
   xor     ebp, ebx
   add     eax, [esp+90h]
   ror     ebx, 2
   add     eax, ebp
   mov     esi, eax
   rol     eax, 5
   mov     ebp, ecx
   add     eax, edx
   xor     ebp, ebx
   add     eax, 6ED9EBA1h
   xor     ebp, edi
   add     eax, [esp+94h]
   ror     edi, 2
   add     eax, ebp
   mov     edx, eax
   rol     eax, 5
   mov     ebp, ebx
   add     eax, ecx
   xor     ebp, edi
   add     eax, 6ED9EBA1h
   xor     ebp, esi
   add     eax, [esp+98h]
   ror     esi, 2
   add     eax, ebp
   mov     ecx, eax
   rol     eax, 5
   mov     ebp, edi
   add     eax, ebx
   xor     ebp, esi
   add     eax, 6ED9EBA1h
   xor     ebp, edx
   add     eax, [esp+9Ch]
   ror     edx, 2
   add     eax, ebp
   mov     ebx, eax
   rol     eax, 5
   mov     ebp, esi
   add     eax, edi
   xor     ebp, edx
   add     eax, 6ED9EBA1h
   xor     ebp, ecx
   add     eax, [esp+0A0h]
   ror     ecx, 2
   add     eax, ebp
   mov     ebp, edx
   mov     edi, eax
   rol     eax, 5
   xor     ebp, ecx
   add     eax, esi
   and     ebp, ebx
   add     eax, 8F1BBCDCh
   mov     esi, ecx
   add     eax, [esp+0A4h]
   and     esi, edx
   xor     ebp, esi
   ror     ebx, 2
   add     eax, ebp
   mov     ebp, ecx
   mov     esi, eax
   rol     eax, 5
   xor     ebp, ebx
   add     eax, edx
   and     ebp, edi
   add     eax, 8F1BBCDCh
   mov     edx, ebx
   add     eax, [esp+0A8h]
   and     edx, ecx
   xor     ebp, edx
   ror     edi, 2
   add     eax, ebp
   mov     ebp, ebx
   mov     edx, eax
   rol     eax, 5
   xor     ebp, edi
   add     eax, ecx
   and     ebp, esi
   add     eax, 8F1BBCDCh
   mov     ecx, edi
   add     eax, [esp+0ACh]
   and     ecx, ebx
   xor     ebp, ecx
   ror     esi, 2
   add     eax, ebp
   mov     ebp, edi
   mov     ecx, eax
   rol     eax, 5
   xor     ebp, esi
   add     eax, ebx
   and     ebp, edx
   add     eax, 8F1BBCDCh
   mov     ebx, esi
   add     eax, [esp+0B0h]
   and     ebx, edi
   xor     ebp, ebx
   ror     edx, 2
   add     eax, ebp
   mov     ebp, esi
   mov     ebx, eax
   rol     eax, 5
   xor     ebp, edx
   add     eax, edi
   and     ebp, ecx
   add     eax, 8F1BBCDCh
   mov     edi, edx
   add     eax, [esp+0B4h]
   and     edi, esi
   xor     ebp, edi
   ror     ecx, 2
   add     eax, ebp
   mov     ebp, edx
   mov     edi, eax
   rol     eax, 5
   xor     ebp, ecx
   add     eax, esi
   and     ebp, ebx
   add     eax, 8F1BBCDCh
   mov     esi, ecx
   add     eax, [esp+0B8h]
   and     esi, edx
   xor     ebp, esi
   ror     ebx, 2
   add     eax, ebp
   mov     ebp, ecx
   mov     esi, eax
   rol     eax, 5
   xor     ebp, ebx
   add     eax, edx
   and     ebp, edi
   add     eax, 8F1BBCDCh
   mov     edx, ebx
   add     eax, [esp+0BCh]
   and     edx, ecx
   xor     ebp, edx
   ror     edi, 2
   add     eax, ebp
   mov     ebp, ebx
   mov     edx, eax
   rol     eax, 5
   xor     ebp, edi
   add     eax, ecx
   and     ebp, esi
   add     eax, 8F1BBCDCh
   mov     ecx, edi
   add     eax, [esp+0C0h]
   and     ecx, ebx
   xor     ebp, ecx
   ror     esi, 2
   add     eax, ebp
   mov     ebp, edi
   mov     ecx, eax
   rol     eax, 5
   xor     ebp, esi
   add     eax, ebx
   and     ebp, edx
   add     eax, 8F1BBCDCh
   mov     ebx, esi
   add     eax, [esp+0C4h]
   and     ebx, edi
   xor     ebp, ebx
   ror     edx, 2
   add     eax, ebp
   mov     ebp, esi
   mov     ebx, eax
   rol     eax, 5
   xor     ebp, edx
   add     eax, edi
   and     ebp, ecx
   add     eax, 8F1BBCDCh
   mov     edi, edx
   add     eax, [esp+0C8h]
   and     edi, esi
   xor     ebp, edi
   ror     ecx, 2
   add     eax, ebp
   mov     ebp, edx
   mov     edi, eax
   rol     eax, 5
   xor     ebp, ecx
   add     eax, esi
   and     ebp, ebx
   add     eax, 8F1BBCDCh
   mov     esi, ecx
   add     eax, [esp+0CCh]
   and     esi, edx
   xor     ebp, esi
   ror     ebx, 2
   add     eax, ebp
   mov     ebp, ecx
   mov     esi, eax
   rol     eax, 5
   xor     ebp, ebx
   add     eax, edx
   and     ebp, edi
   add     eax, 8F1BBCDCh
   mov     edx, ebx
   add     eax, [esp+0D0h]
   and     edx, ecx
   xor     ebp, edx
   ror     edi, 2
   add     eax, ebp
   mov     ebp, ebx
   mov     edx, eax
   rol     eax, 5
   xor     ebp, edi
   add     eax, ecx
   and     ebp, esi
   add     eax, 8F1BBCDCh
   mov     ecx, edi
   add     eax, [esp+0D4h]
   and     ecx, ebx
   xor     ebp, ecx
   ror     esi, 2
   add     eax, ebp
   mov     ebp, edi
   mov     ecx, eax
   rol     eax, 5
   xor     ebp, esi
   add     eax, ebx
   and     ebp, edx
   add     eax, 8F1BBCDCh
   mov     ebx, esi
   add     eax, [esp+0D8h]
   and     ebx, edi
   xor     ebp, ebx
   ror     edx, 2
   add     eax, ebp
   mov     ebp, esi
   mov     ebx, eax
   rol     eax, 5
   xor     ebp, edx
   add     eax, edi
   and     ebp, ecx
   add     eax, 8F1BBCDCh
   mov     edi, edx
   add     eax, [esp+0DCh]
   and     edi, esi
   xor     ebp, edi
   ror     ecx, 2
   add     eax, ebp
   mov     ebp, edx
   mov     edi, eax
   rol     eax, 5
   xor     ebp, ecx
   add     eax, esi
   and     ebp, ebx
   add     eax, 8F1BBCDCh
   mov     esi, ecx
   add     eax, [esp+0E0h]
   and     esi, edx
   xor     ebp, esi
   ror     ebx, 2
   add     eax, ebp
   mov     ebp, ecx
   mov     esi, eax
   rol     eax, 5
   xor     ebp, ebx
   add     eax, edx
   and     ebp, edi
   add     eax, 8F1BBCDCh
   mov     edx, ebx
   add     eax, [esp+0E4h]
   and     edx, ecx
   xor     ebp, edx
   ror     edi, 2
   add     eax, ebp
   mov     ebp, ebx
   mov     edx, eax
   rol     eax, 5
   xor     ebp, edi
   add     eax, ecx
   and     ebp, esi
   add     eax, 8F1BBCDCh
   mov     ecx, edi
   add     eax, [esp+0E8h]
   and     ecx, ebx
   xor     ebp, ecx
   ror     esi, 2
   add     eax, ebp
   mov     ebp, edi
   mov     ecx, eax
   rol     eax, 5
   xor     ebp, esi
   add     eax, ebx
   and     ebp, edx
   add     eax, 8F1BBCDCh
   mov     ebx, esi
   add     eax, [esp+0ECh]
   and     ebx, edi
   xor     ebp, ebx
   ror     edx, 2
   add     eax, ebp
   mov     ebp, esi
   mov     ebx, eax
   rol     eax, 5
   xor     ebp, edx
   add     eax, edi
   and     ebp, ecx
   add     eax, 8F1BBCDCh
   mov     edi, edx
   add     eax, [esp+0F0h]
   and     edi, esi
   xor     ebp, edi
   ror     ecx, 2
   add     eax, ebp
   mov     edi, eax
   rol     eax, 5
   mov     ebp, edx
   add     eax, esi
   xor     ebp, ecx
   add     eax, 0CA62C1D6h
   xor     ebp, ebx
   add     eax, [esp+0F4h]
   ror     ebx, 2
   add     eax, ebp
   mov     esi, eax
   rol     eax, 5
   mov     ebp, ecx
   add     eax, edx
   xor     ebp, ebx
   add     eax, 0CA62C1D6h
   xor     ebp, edi
   add     eax, [esp+0F8h]
   ror     edi, 2
   add     eax, ebp
   mov     edx, eax
   rol     eax, 5
   mov     ebp, ebx
   add     eax, ecx
   xor     ebp, edi
   add     eax, 0CA62C1D6h
   xor     ebp, esi
   add     eax, [esp+0FCh]
   ror     esi, 2
   add     eax, ebp
   mov     ecx, eax
   rol     eax, 5
   mov     ebp, edi
   add     eax, ebx
   xor     ebp, esi
   add     eax, 0CA62C1D6h
   xor     ebp, edx
   add     eax, [esp+100h]
   ror     edx, 2
   add     eax, ebp
   mov     ebx, eax
   rol     eax, 5
   mov     ebp, esi
   add     eax, edi
   xor     ebp, edx
   add     eax, 0CA62C1D6h
   xor     ebp, ecx
   add     eax, [esp+104h]
   ror     ecx, 2
   add     eax, ebp
   mov     edi, eax
   rol     eax, 5
   mov     ebp, edx
   add     eax, esi
   xor     ebp, ecx
   add     eax, 0CA62C1D6h
   xor     ebp, ebx
   add     eax, [esp+108h]
   ror     ebx, 2
   add     eax, ebp
   mov     esi, eax
   rol     eax, 5
   mov     ebp, ecx
   add     eax, edx
   xor     ebp, ebx
   add     eax, 0CA62C1D6h
   xor     ebp, edi
   add     eax, [esp+10Ch]
   ror     edi, 2
   add     eax, ebp
   mov     edx, eax
   rol     eax, 5
   mov     ebp, ebx
   add     eax, ecx
   xor     ebp, edi
   add     eax, 0CA62C1D6h
   xor     ebp, esi
   add     eax, [esp+110h]
   ror     esi, 2
   add     eax, ebp
   mov     ecx, eax
   rol     eax, 5
   mov     ebp, edi
   add     eax, ebx
   xor     ebp, esi
   add     eax, 0CA62C1D6h
   xor     ebp, edx
   add     eax, [esp+114h]
   ror     edx, 2
   add     eax, ebp
   mov     ebx, eax
   rol     eax, 5
   mov     ebp, esi
   add     eax, edi
   xor     ebp, edx
   add     eax, 0CA62C1D6h
   xor     ebp, ecx
   add     eax, [esp+118h]
   ror     ecx, 2
   add     eax, ebp
   mov     edi, eax
   rol     eax, 5
   mov     ebp, edx
   add     eax, esi
   xor     ebp, ecx
   add     eax, 0CA62C1D6h
   xor     ebp, ebx
   add     eax, [esp+11Ch]
   ror     ebx, 2
   add     eax, ebp
   mov     esi, eax
   rol     eax, 5
   mov     ebp, ecx
   add     eax, edx
   xor     ebp, ebx
   add     eax, 0CA62C1D6h
   xor     ebp, edi
   add     eax, [esp+120h]
   ror     edi, 2
   add     eax, ebp
   mov     edx, eax
   rol     eax, 5
   mov     ebp, ebx
   add     eax, ecx
   xor     ebp, edi
   add     eax, 0CA62C1D6h
   xor     ebp, esi
   add     eax, [esp+124h]
   ror     esi, 2
   add     eax, ebp
   mov     ecx, eax
   rol     eax, 5
   mov     ebp, edi
   add     eax, ebx
   xor     ebp, esi
   add     eax, 0CA62C1D6h
   xor     ebp, edx
   add     eax, [esp+128h]
   ror     edx, 2
   add     eax, ebp
   mov     ebx, eax
   rol     eax, 5
   mov     ebp, esi
   add     eax, edi
   xor     ebp, edx
   add     eax, 0CA62C1D6h
   xor     ebp, ecx
   add     eax, [esp+12Ch]
   ror     ecx, 2
   add     eax, ebp
   mov     edi, eax
   rol     eax, 5
   mov     ebp, edx
   add     eax, esi
   xor     ebp, ecx
   add     eax, 0CA62C1D6h
   xor     ebp, ebx
   add     eax, [esp+130h]
   ror     ebx, 2
   add     eax, ebp
   mov     esi, eax
   rol     eax, 5
   mov     ebp, ecx
   add     eax, edx
   xor     ebp, ebx
   add     eax, 0CA62C1D6h
   xor     ebp, edi
   add     eax, [esp+134h]
   ror     edi, 2
   add     eax, ebp
   mov     edx, eax
   rol     eax, 5
   mov     ebp, ebx
   add     eax, ecx
   xor     ebp, edi
   add     eax, 0CA62C1D6h
   xor     ebp, esi
   add     eax, [esp+138h]
   ror     esi, 2
   add     eax, ebp
   mov     ecx, eax
   rol     eax, 5
   mov     ebp, edi
   add     eax, ebx
   xor     ebp, esi
   add     eax, 0CA62C1D6h
   xor     ebp, edx
   add     eax, [esp+13Ch]
   ror     edx, 2
   add     eax, ebp
   mov     ebx, eax
   rol     eax, 5
   mov     ebp, esi
   add     eax, edi
   xor     ebp, edx
   add     eax, 0CA62C1D6h
   xor     ebp, ecx
   add     eax, [esp+140h]
   ror     ecx, 2
   add     eax, ebp
   mov     ebp, [esp+168h]
   add     [ebp+8], eax
   add     [ebp+0Ch], ebx
   add     [ebp+10h], ecx
   add     [ebp+14h], edx
   add     [ebp+18h], esi
   retn    

align 10h

global _SHA1_Add_p5@12

_SHA1_Add_p5@12:
   pusha   
   sub     esp, 140h
   mov     ecx, [esp+160h+0Ch]
   and     ecx, ecx
   jz     get_out
   xor     edx, edx
   mov     ebp, [esp+160h+8h]
   mov     edi, [esp+160h+4h]
   mov     ebx, [edi]
   mov     eax, ebx
   add     ebx, ecx
   mov     [edi], ebx
   adc     [edi+4], edx
   and     eax, 3Fh
   jnz     partial_buffer
full_blocks:
   mov     ecx, [esp+160h+0Ch]
   and     ecx, ecx
   jz      get_out
   sub     ecx, 40h
   jb      end_of_stream
   mov     [esp+160h+0Ch], ecx
   call    _SHA_Compile_p5@0
   mov     ebp, [esp+160h+8h]
   add     ebp, 40h
   mov     [esp+160h+8h], ebp
   jmp     full_blocks
end_of_stream:
   mov     edi, [esp+160h+4h]
   mov     esi, ebp
   lea     edi, [edi+1Ch]
   add     ecx, 40h
   rep movsb
   jmp     get_out
partial_buffer:
   add     ecx, eax
   cmp     ecx, 40h
   jb      short_stream
   mov     ecx, 0FFFFFFC0h
   add     ecx, eax
   add     [esp+160h+0Ch], ecx
loc:
   mov     bl, [ebp+0]
   inc     ebp
   mov     [edi+ecx+5Ch], bl
   inc     ecx
   jnz     loc
   mov     [esp+160h+8h], ebp
   lea     ebp, [edi+1Ch]
   call    _SHA_Compile_p5@0
   mov     ebp, [esp+160h+8h]
   jmp     full_blocks
short_stream:
   sub     ecx, eax
   mov     esi, ebp
   lea     edi, [edi+eax+1Ch]
   rep movsb
get_out:
   add     esp, 140h
   popa    
   retn    0Ch
