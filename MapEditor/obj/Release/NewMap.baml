   M S B A M L   `   `   `  ���� D  @MapEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:MapEditor	MapEditor      MapEditor.NewMap   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     )localclr-namespace:MapEditor       � Title$  Create A New Map�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterOwner=�$	��300��$	��200��.���  � UseLayoutRounding .  $��2,0,0,0q�.+��� -    ��Name$ mapNameTextBox��$
��Left=�$��23��$��
129,23,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��147�� �� 	MaxLength$ 32���� -   $ xSizeTextBox��$
��Left=�$��23��$��
129,67,0,0q�$
 Wrap=�$	��Top=�$��86��$ 3���� -   $ ySizeTextBox��$
��Left=�$��23��$��
129,96,0,0q�$
 Wrap=�$	��Top=�$��86��$ 3���� -    ��Name$ validateButton��$��Validate��$
��Left=�$��114,131,0,0q�$	��Top=�$��75���� -   $ cancelButton��$��Cancel��$
��Left=�$��201,131,0,0q�$	��Top=�$��75���� -    ��Name$ mapNameLabel��$��Name of the map :��$
��Left=�$��0,23,0,0q�$	��Top=�$	��124��$��23��$��Right=�$��0q�$��Center=�$��13(�$��Medium"��� -   $ 	sizeLabel��$��Size :��$
��Left=�$��	14,51,0,0q�$	��Top=�$��34��$��23��$��0q�$��Center=�$��13(�$��Medium"��� -   $ 
ySizeLabel��$	��Y :��$
��Left=�$��
105,95,0,0q�$	��Top=�$��19��$��23��$��0q�$��Center=�$��13(�$��Medium"�$��Right=��� -	   $ 
xSizeLabel��$	��X :��$
��Left=�$��
105,67,0,0q�$	��Top=�$��19��$��23��$��0q�$��Center=�$��13(�$��Medium"�$��Right=�