   M S B A M L   `   `   `  ���� D  @MapEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:MapEditor	MapEditor  $    MapEditor.TilePropertiesWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     )localclr-namespace:MapEditor       � Title$#  Water Drop Project Map Editor��$	��320��$	��400�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=�.��� .+��� -    ��Name$ 	mainLabel��$��Content��$��Center=�$��107,10,133.6,0q�$	��Top=�$	��153��$��25��$��Center=�$��0q�$��Center=�$��16(�$��Medium"��� -   $ 	textLabel��$��Text :��$
��Left=�$��	10,51,0,0q�$	��Top=�$��60��$��25��$��Center=�$��0q�$��Center=�$��14(�$��Medium"��� -    ��Name$ textTextBox��$
��Left=�$��70��$��	75,51,0,0q� �� TextWrapping$
 Wrap=�$�� ��$	��Top=�$	��300���� -   $ x1Label��$
��x1 :��$
��Left=�$��
10,133,0,0q�$	��Top=�$��60��$��25��$��Center=�$��0q�$��Center=�$��14(�$��Medium"� J� RenderTransformOrigin$ 0.766,0.597&��� -   $ 	x1TextBox��$
��Left=�$��25��$��
75,133,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��110�� �� 
IsReadOnly . �� -    ��Name$ validateButton_Copy��$��Validate��$
��Left=�$��120,253,0,0q�$	��Top=�$��75���� -   $ deleteButton��$��Delete��$
��Left=�$��210,253,0,0q�$	��Top=�$��75���� -   $ cancelButton��$��Cancel��$
��Left=�$��300,253,0,0q�$	��Top=�$��75���� -	   $ y1Label��$
��y1 :��$
��Left=�$��200,133,0,0q�$	��Top=�$��60��$��25��$��Center=�$��0q�$��Center=�$��14(�$��Medium"�$ 0.766,0.597&��� -
   $ 	y1TextBox��$
��Left=�$��25��$��265,133,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��110�� . �� -   $ x2Label��$
��x2 :��$
��Left=�$��
10,173,0,0q�$	��Top=�$��60��$��25��$��Center=�$��0q�$��Center=�$��14(�$��Medium"�$ 0.766,0.597&��� -   $ 	x2TextBox��$
��Left=�$��25��$��
75,173,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��110���� -   $ y2Label��$
��y2 :��$
��Left=�$��200,173,0,0q�$	��Top=�$��60��$��25��$��Center=�$��0q�$��Center=�$��14(�$��Medium"�$ 0.766,0.597&��� -   $ 	y2TextBox��$
��Left=�$��25��$��265,173,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��110���� -   	 ��Name$	 spritePreview�����	#FFF4F4F5$
��Left=�$��24��$��
265,11,0,0q�
���   �$	��Top=�$��24���� -   $ 	sizeLabel��$��Size :��$
��Left=�$��
10,213,0,0q�$	��Top=�$��60��$��25��$��Center=�$��0q�$��Center=�$��14(�$��Medium"�$ 0.766,0.597&��� -   $ sizeTextBox��$
��Left=�$��25��$��
75,213,0,0q�$
 Wrap=�$�� ��$	��Top=�$��90���� -   $ orientationLabel��$��Orientation :��$
��Left=�$��170,213,0,0q�$	��Top=�$��90��$��25��$��Center=�$��0q�$��Center=�$��14(�$��Medium"�$ 0.766,0.597&��� -   
 ��Name$
 orientationSelect��$
��Left=�$��265,215,0,0q�$	��Top=�$	��110��