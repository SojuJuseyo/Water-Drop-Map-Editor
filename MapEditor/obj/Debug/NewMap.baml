   M S B A M L   `   `   `  ����D  @MapEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:MapEditor	MapEditor  5   	       MapEditor.NewMap   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:MapEditor     5   	     � Title$  Create A New Map��5   	    � 
ResizeMode$ NoResize=�6"    � WindowStartupLocation$ CenterOwner=�68   $	��300��6\   $	��200��6h   .��5	      �  � UseLayoutRounding .  $��2,0,0,0q�6%   .+�5
   
   �� -   5         ��Name$ mapNameTextBox��5
      $
��Left=�6*   $��23��6E   $��
129,23,0,0q�6Q    �� TextWrapping$
 Wrap=�6e   $	��Top=�6y   $	��147��6�    �� 	MaxLength$ 32��6�   6�   �� 5   
   -   5        $ xSizeTextBox��5      $
��Left=�6(   $��23��6C   $��
129,67,0,0q�6O   $
 Wrap=�6c   $	��Top=�6w   $��86��6�   $ 3��6�   6�   �� 5   
   -   5        $ ySizeTextBox��5      $
��Left=�6(   $��23��6C   $��
129,96,0,0q�6O   $
 Wrap=�6c   $	��Top=�6w   $��86��6�   $ 3��6�   6�   �� 5   
   -   5         ��Name$ validateButton��5      $��Validate��6)   $
��Left=�6<   $��114,131,0,0q�6W   $	��Top=�6l   $��75��6�   6�   �� 5   
   -   5        $ cancelButton��5      $��Cancel��6'   $
��Left=�68   $��201,131,0,0q�6S   $	��Top=�6h   $��75��6�   6�   �� 5   
   -   5         ��Name$ mapNameLabel��5      $��Name of the map :��6&   $
��Left=�6B   $��0,23,0,0q�6]   $	��Top=�6o   $	��124��6�   $��23��6�   $��Right=�6�   $��0q�6�   $��Center=�6�   $��13(�6�   $��Medium"�6�   �� 5   
   -   5        $ 	sizeLabel��5      $��Size :��6#   $
��Left=�64   $��	14,51,0,0q�6O   $	��Top=�6b   $��34��6z   $��23��6�   $��0q�6�   $��Center=�6�   $��13(�6�   $��Medium"�6�   �� 5   
   -   5        $ 
ySizeLabel��5      $	��Y :��6$   $
��Left=�62   $��
105,95,0,0q�6M   $	��Top=�6a   $��19��6y   $��23��6�   $��0q�6�   $��Center=�6�   $��13(�6�   $��Medium"�6�   $��Right=�6�   �� 5   
   -	   5        $ 
xSizeLabel��5      $	��X :��6$   $
��Left=�62   $��
105,67,0,0q�6M   $	��Top=�6a   $��19��6y   $��23��6�   $��0q�6�   $��Center=�6�   $��13(�6�   $��Medium"�6�   $��Right=�6�   5      5      