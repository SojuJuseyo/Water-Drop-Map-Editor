   M S B A M L   `   `   `  ���� D  @MapEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:MapEditor	MapEditor      MapEditor.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     )localclr-namespace:MapEditor     -     � Title$#  Water Drop Project Map Editor��$	��620��$
��1024��$	��620��$
��1024��.��� .+�o� $	��Top=�$��0q�$��25��$	��Top=�$��0,0,0,1q����	#FF9C9898.@�m� $��_File��$��0q�$��7,2,8,6q�.@�m� -    m�Name$ 	newButton��$
��_New��m� -   $ 
openButton��$��_Open��m� -   $ 
saveButton��$��_Save��{�.  �� -    ��Name$ ScrollViewer��|�.  $��
430,30,0,0q�$
��Left=�$	��Top=�$
��Auto=�$
��Auto=���. $	��550��$	��550��.���� -    ��Name$ mapGrid��$��0,0,5,0q�$
��Left=�$	��Top=�$
��Auto������ *ActualWidth+ �� ElementName$ ScrollViewer���� -    ��Name$ gridSplitter��$
��Left=�$��
420,27,0,0q�$	��Top=�$��2�����	#FFCCCCCC$	��554��$y�Hidden=�|�.  {�.  ���� *ActualHeight+$ ScrollViewer���� -    ��Name$ selectedSpriteLabel��$��Currently selected :��$
��Left=�$��	10,37,0,0q�$	��Top=�$��35��$	��124��$��14(�$��Italic$�$��Center=�$y�Hidden=��� -	    ��Name$ selectedSprite��
�������$
��Left=�$��35��$��
138,37,0,0q����	#FFA6A4A4$	��Top=�$��35��$��0���� -
    ��Name$ backgroundButton��$
��Left=�$	��Top=�$��
335,37,0,0q�$y�Hidden=�$��35��$��35��.���� $#��/Resources/backgroundIcon.png���� -   $ audioButton��$
��Left=�$	��Top=�$��
375,37,0,0q�$y�Hidden=�$��35��$��35��.���� $��/Resources/audioIcon.png���� -   $ tileSelectionPanel��$
��Left=�$	��491��$��	10,91,0,0q�$	��Top=�$	��425��