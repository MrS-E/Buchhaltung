   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.KontoNeu   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	     � Title$  Neues Konto��5   	   $	��200��5	   	   $	��300��5
   	    � SizeToContent$ WidthAndHeight=�5   	    � 
ResizeMode$ NoResize=�5   	    � ShowInTaskbar$ False��5   	    � WindowStartupLocation$ CenterOwner=�5   	    +� FocusedElement 5   	   ��  �� ElementName$ leftMarginTextBox��.��5   
   � $��10q�6    � 	Resources 5       �� 
TargetType	 �� Property
 �� Value��%v  '��       '�� v     '�� �     �� 5      " ��6   .�5      �� 	 � ��$
 0,3,5,5q�6/   �� 5      	 � ��$
 0,0,0,5q�60   5      �� 5      " ��6   .�5      �� 	 � ��$
 0,0,0,5q�6/   5      �� 5      " ��6   .�5      �� 	 � ��$
 70��6.   �� 5      	 � ��$
 25��6/   �� 5      	 � ��$
 5,0,0,0q�6/   5      5       � ColumnDefinitions 5!      �� 5"      $
��Auto �6#   �� 5#       � RowDefinitions 5&      �� 5'      $
��Auto �6    �� 5(      $
��Auto �6    �� 5)      $
��Auto �6    �� 5*      .+�5.   
   �� $��0��6   $��0��6    .��6-   Name des Kontos6>   �� 5/      -   5        $��	NameKonto��5/      $��1��6'   $��0��67   �� 51   
   $��0��6   $��1��6    .��6-   Art des Kontos6=   �� 52   
   -   5        $��	KatiKonto��52      $��Center=�6$   $��0,2,0,5q�6A   $��1��6R   $	��188��6_   $��1��6k   .@�53      �� $��Aktiv��6   �� 54      $��Passiv��6   �� 55      $��Ertrag��6   �� 56      $��Aufwand��6   57      �� 59      $��0��6   $��0,60,0,0q�6)   $��2��6;   $��5��6O    �� Orientation$ 
Horizontal=�6\   $��Right=�6u   .+�5:      �� -   5        $��okButton��5:       �� 	IsDefault$
 True��6)   .��6Q   OK6U   �� 5;      -   5        $��cancelButton��5;       �� IsCancel$
 True��6-   .��6X   Cancel6`   5<      5=      5>      