   M S B A M L   `   `   `  ����D  @MapEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:MapEditor	MapEditor  5   	       MapEditor.SavePopup   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:MapEditor     5   	   -   5          � Title$  Save your map��5   	   $	��200��6   $	��300��6,    � WindowStartupLocation$ CenterOwner=�68    � 
ResizeMode$ NoResize=�6\   .��5	      � .+�5
   
   �� -   5         ��Name$ 	saveLabel��5
      $5��/Your map has unsaved changes !
Save your map ?��6#   $
��Left=�6e   $��	10,27,0,0q�6�   $	��Top=�6�   $��44��6�   $	��260��6�   $��13(�6�   $��Medium"�6�   $��Center=�6�   �� 5   
   -   5         ��Name$ 
saveButton��5      $
��Save��6%   $
��Left=�64   $��109,121,0,0q�6O   $	��Top=�6d   $��75��6|   $��30��6�   6�   �� 5   
   -   5        $ 
exitButton��5      $
��Exit��6%   $
��Left=�64   $��194,121,0,0q�6O   $	��Top=�6d   $��75��6|   $��30��6�   6�   �� 5   
   -   5        $ currentLocationLabel��5      $
��Left=�6.   $��	10,80,0,0q�6I   $	��Top=�6\   $��28��6t   $	��260��6�   $��11(�6�   $��Medium"�6�   $��Center=�6�   .��5      �� -   5         ��Name$ currentLocationTextBlock��5      $��CharacterEllipsis=�6:   6^   5      �� 5   
   -   5        $ changeButton��5      $��Change path��6'   $
��Left=�6=   $��
25,121,0,0q�6X   $	��Top=�6l   $��75��6�   $��30��6�   6�   5      5      