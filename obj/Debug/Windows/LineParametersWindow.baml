   M S B A M L   `   `   `  ����  MachPlotAsynchro,    &MachPlotNamespace.LineParametersWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   X TXceed.Wpf.Toolkit, Version=2.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	     � Title$  Zmiana wyglądu linii��5   	   $	��200��6&   $	��300��63   .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   6,   �� 5	      $
��auto �6   6,   �� 5
      $
��auto �6   6,   �� 5      $
��auto �6   6,   �� 5      $
��auto �6   6,   .+�5   
   �� -   5         ��Name$ lineColorLabel��5      $��Wybierz kolor linii:��6(   $��0��6G   $
��Bold"�6T   $��14(�6f   #  Xceed.Wpf.Toolkit.ColorPicker  5   
   -   5          Name$ ClrPcker_Background��5      $��30��5      $��1��5         SelectedColor$ 
BlueVioletX�5      �� 5   
   $��Wybierz grubość linii:��6   $��2��61   $
��Bold"�6>   $��14(�6P   �� 5   
   -   5         ��Name$ strokeWeightTextBox��5      $��3��6/   $��30��6<   6v   �� 5   
   -   5         ��Name$ acceptButton��5      $��
Zatwierdź��6'   $��30��6;   $��Center=�6G   $��Center=�6d   $��4��6   $��0,5,0,0q�6�   6�   5      5      