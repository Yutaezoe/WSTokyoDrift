import pandas as pd
import numpy as np


#pathの設定
user_ID=13068
path=f'C:/Users/{user_ID}/dojo/WSTokyoDrift0928/WSTokyoDrift/TokyoDrift/Assets/CSV'

#ファイル読み込み
headers=['Mo_ID','Ta_ID','Distance']
df=pd.read_csv(f'{path}/toPython.csv',encoding='UTF-8',names=headers)

#mover_ID取得
col_list=df['Mo_ID'].unique()

#条件分岐
for i in range(len(col_list)):
    choi=df[df['Mo_ID']==col_list[i]]
    choi=choi.set_index('Ta_ID')
    choi=choi.rename(columns={'Distance':col_list[i]})
    choi=choi.drop('Mo_ID',axis=1)
    
    if  i ==0:
        prep_df=choi
    elif 1 <= i <= len(col_list):
        prep_df=pd.merge(prep_df,choi,how='outer',left_index=True,right_index=True)

#print(prep_df)
########################################################################################################
#Data_select_sprict
########################################################################################################

df_c=prep_df #コピー下で作業

#最小値をみつけて、そのMover_IDとTarget_IDを削除する関数
def func(df_c):
    new_df=pd.DataFrame()
    for col_name in df_c.columns:  #各moverの最短を探索するループ    
        total_min = df_c.min().min()  #df_c中の最小値を見つける
        df_pick=df_c[col_name].sort_values()   #moverの若いほうからソートする
        mover_min=df_pick.values[0]
        if total_min  == mover_min:  #分岐条件１：表中の最小値と指定のmoverでソートした時の最小値が同じだった場合
            mover_second=df_pick.values[1]
            if mover_min != mover_second:  #分岐条件２：そのmoverで2番目に近い経路が同じ重さでなかった場合
                new_df=new_df.append(df_pick.head(1)) #new_dfにmover、pick場所、重みを格納
                df_c=df_c.drop(df_pick.name,axis=1) #抽出したmoverをリストから除外
                df_c=df_c.drop(df_pick.index[0],axis=0) #抽出したTargetをリストから除外
        elif col_name == list(df_c.columns)[-1]:
            new_df=new_df.append(df_pick.head(1))   #最後に検索したmoverをアサイン
            df_c=df_c.drop(df_pick.name,axis=1)
            df_c=df_c.drop(df_pick.index[0],axis=0)
 
    return df_c, new_df


#関数を呼び出す
df_c1, new_df1 = func(df_c)

#ループ条件(要ループ化。暫定Max.20台)
if len(df_c1.columns) != 0:
    df_c2, new_df2 = func(df_c1)
    final_new_df=pd.concat([new_df1,new_df2],axis=1)
    if len(df_c2.columns) != 0:
        df_c3, new_df3 = func(df_c2)
        final_new_df=pd.concat([new_df1,new_df3],axis=1)
        if len(df_c3.columns) != 0:
            df_c4, new_df4 = func(df_c3)
            final_new_df=pd.concat([new_df1,new_df4],axis=1)
            if len(df_c4.columns) != 0:
                df_c5, new_df5 = func(df_c4)
                final_new_df=pd.concat([new_df1,new_df5],axis=1)
                if len(df_c5.columns) != 0:
                    df_c6, new_df6 = func(df_c5)
                    final_new_df=pd.concat([new_df1,new_df6],axis=1)
                    if len(df_c6.columns) != 0:
                        df_c7, new_df7 = func(df_c6)
                        final_new_df=pd.concat([new_df1,new_df7],axis=1)
                        if len(df_c7.columns) != 0:
                            df_c8, new_df8 = func(df_c7)
                            final_new_df=pd.concat([new_df1,new_df8],axis=1)
                            if len(df_c8.columns) != 0:
                                df_c9, new_df9 = func(df_c8)
                                final_new_df=pd.concat([new_df1,new_df9],axis=1)
                                if len(df_c9.columns) != 0:
                                    df_c10, new_df10 = func(df_c9)
                                    final_new_df=pd.concat([new_df1,new_df10],axis=1)
                                    if len(df_c10.columns) != 0:
                                        df_c11, new_df11 = func(df_c10)
                                        final_new_df=pd.concat([new_df1,new_df11],axis=1)
                                    if len(df_c11.columns) != 0:
                                        df_c12, new_df12 = func(df_c11)
                                        final_new_df=pd.concat([new_df1,new_df12],axis=1)
                                        if len(df_c12.columns) != 0:
                                            df_c13, new_df13 = func(df_c12)
                                            final_new_df=pd.concat([new_df1,new_df13],axis=1)
                                            if len(df_c13.columns) != 0:
                                                df_c14, new_df14 = func(df_c13)
                                                final_new_df=pd.concat([new_df1,new_df14],axis=1)
                                                if len(df_c14.columns) != 0:
                                                    df_c15, new_df15 = func(df_c14)
                                                    final_new_df=pd.concat([new_df1,new_df15],axis=1)
                                                    if len(df_c15.columns) != 0:
                                                        df_c16, new_df16 = func(df_c15)
                                                        final_new_df=pd.concat([new_df1,new_df16],axis=1)
                                                        if len(df_c16.columns) != 0:
                                                            df_c17, new_df17 = func(df_c16)
                                                            final_new_df=pd.concat([new_df1,new_df17],axis=1)
                                                            if len(df_c17.columns) != 0:
                                                                df_c18, new_df18 = func(df_c17)
                                                                final_new_df=pd.concat([new_df1,new_df18],axis=1)
                                                                if len(df_c18.columns) != 0:
                                                                    df_c19, new_df19 = func(df_c18)
                                                                    final_new_df=pd.concat([new_df1,new_df19],axis=1)
                                                                    if len(df_c19.columns) != 0:
                                                                        df_c20, new_df20 = func(df_c19)
                                                                        final_new_df=pd.concat([new_df1,new_df20],axis=1)
else:
    final_new_df=new_df1



#index,columns名をリスト化
list_mo=list(final_new_df.index)
list_ta=list(final_new_df.columns)

#df形式に変更
out_data=np.array([list_mo,list_ta])
out_df=pd.DataFrame(out_data).T
#out_df=out_df.sort_values[0]
#print(out_df)

#保存と任意No.の出力
out_df.to_csv(f'{path}/toCS.csv',index=False,header=False,encoding='UTF-8')
print(999)