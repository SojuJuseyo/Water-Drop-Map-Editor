   M S B A M L   `   `   `  ����D  @MapEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:MapEditor	MapEditor  5   	       MapEditor.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:MapEditor     5   	   -   5          � Title$#  Water Drop Project Map Editor��5   	   $	��620��6/   $
��1024��6<   $	��620��6I   $
��1024��6Y   .��5	      � .+�5
   
   o� $	��Top=�6   $��0q�6$   $��25��6/   $	��Top=�6;   $��0,0,0,1q�6S   ���	#FF9C9898.@�5      m� $��_File��6   $��0q�6&   $��7,2,8,6q�61   .@�5      m� -   5         m�Name$ 	newButton��5      $
��_New��6.   6<   m� 5      -   5        $ 
openButton��5      $��_Open��6/   6>   m� 5      -   5        $ 
saveButton��5      $��_Save��6/   {�.  6W   5      5      �� 5   
   -   5         ��Name$ ScrollViewer��5   p   |�.  $��
430,30,0,0q�6)   $
��Left=�6=   $	��Top=�6X   $
��Auto=�6�   $
��Auto=�6�   ��. $	��550��6�   $	��550��6�   .��5      �� -   5         ��Name$ mapGrid��5   x   $��0,0,5,0q�6   $
��Left=�6�   $	��Top=�6�   $
��Auto��6�   ��6�   �� *ActualWidth+ �� ElementName$ ScrollViewer��6  5      �� 5   
   -   5         ��Name$ gridSplitter��5      $
��Left=�6-   $��
420,27,0,0q�6H   $	��Top=�6\   $��2��6t   ���	#FFCCCCCC$	��554��6�   $y�Hidden=�6�   |�.  {�.  ��6�   �� *ActualHeight+$ ScrollViewer��6  �� 5   
   -   5         ��Name$ selectedSpriteLabel��5      $��Currently selected :��6-   $
��Left=�6L   $��	10,37,0,0q�6g   $	��Top=�6z   $��35��6�   $	��124��6�   $��14(�6�   $��Italic$�6�   $��Center=�6�   $y�Hidden=�6�   �� 5   
   -	   5         ��Name$ selectedSprite��5      
�������$
��Left=�69   $��35��6T   $��
138,37,0,0q�6`   ���	#FFA6A4A4$	��Top=�6�   $��35��6�   $��0��6�   �� 5   
   -
   5         ��Name$ backgroundButton��5      $
��Left=�6+   $	��Top=�6F   $��
335,37,0,0q�6^   $y�Hidden=�6r   $��35��6�   $��35��6�   .��5      �� $#��/Resources/backgroundIcon.png��6   5      �� 5   
   -   5        $ audioButton��5      $
��Left=�6&   $	��Top=�6A   $��
375,37,0,0q�6Y   $y�Hidden=�6m   $��35��6�   $��35��6�   .��5      �� $��/Resources/audioIcon.png��6   5      �� 5   
   -   5        $ tileSelectionPanel��5      $
��Left=�61   $	��491��6L   $��	10,91,0,0q�6Y   $	��Top=�6l   $	��425��6�   5      5      