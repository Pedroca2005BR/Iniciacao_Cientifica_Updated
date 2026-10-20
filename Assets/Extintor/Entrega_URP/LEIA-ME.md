# Extintores com rótulos — Unity URP

1. Copie a pasta `Entrega_URP` inteira para `Assets` do seu projeto URP.
2. Aguarde a importação e compilação do script de Editor.
3. Execute **Tools > IC > Criar extintores URP**.
4. Na pasta `Gerados_URP`, arraste o prefab A, CO2 ou Po_BC para a cena.
5. Ajuste a escala no objeto raiz. A altura original foi mantida (~4,37 unidades); apenas o modelo foi reposicionado sobre o chão. Exemplo: altura desejada / 4,3732 = escala uniforme aproximada.

O menu cria uma nova pasta a cada execução para preservar ajustes anteriores. Não altera configurações do projeto nem instala pacotes. Não adiciona colliders ou lógica de interação.

## Conteúdo

- `Extintor.fbx`: geometria compartilhada, 11 peças e 4.030 triângulos, com UV do corpo preparado para as novas texturas.
- `Textures/Extintor_*_BaseColor.png`: três imagens 4096 × 4096, contendo pintura de base e rótulo. Usar como Base Map em URP/Lit, com cor branca e sRGB ligado.
- `References`: cópias intactas dos rótulos enviados.
- `Editor/PrepararExtintoresURP.cs`: criação dos materiais e dos três prefabs.
- No diretório superior: `Extintor_Rotulado.blend`, texturas incorporadas e materiais `Corpo_A`, `Corpo_CO2`, `Corpo_Po_BC`; `Previews`, renderizações de conferência; `Scripts`, processo reproduzível.

## Escopo desta versão

Os adesivos estão incorporados na textura de cor: não dependem de decal, transparência, node de projeção nem objeto sobreposto na Unity. O UV do corpo mudou; use o FBX entregue com essas imagens, e não o modelo original.

A pintura mantém a cor e as marcas da textura base. Esta entrega usa acabamento uniforme no corpo (Metallic 0; Smoothness 0,52); os mapas originais de normal, altura e variação de brilho não foram transportados. Peças menores receberam acabamentos simples no lugar dos materiais de teste UV. O mostrador permanece sem arte de escala/ponteiro.

As três variantes compartilham a geometria enviada. Esta etapa não remodela diferenças físicas entre os tipos de extintores e não revisa o conteúdo técnico dos rótulos. As artes foram preservadas.

As imagens 4K são os arquivos mestres para preservar texto. Ajuste Max Size e compressão conforme a legibilidade e o desempenho no dispositivo final; teste especialmente os textos pequenos em VR.

O FBX e as imagens podem ser usados manualmente sem o script: crie um material URP/Lit por variante, atribua a respectiva Base Map e aplique ao objeto `Corpo`. A validação no projeto Unity do usuário ainda é necessária.

Referência de propriedades do shader: https://github.com/Unity-Technologies/Graphics/blob/master/Packages/com.unity.render-pipelines.universal/Shaders/Lit.shader
