import streamlit as st
import yfinance as yf
import plotly.graph_objects as go
import pandas as pd
from pygooglenews import GoogleNews

st.set_page_config(layout="wide", page_title="Sigurds aksjeanalyse")
st.header("Norsk markedsanalyse: Kurs og Hendelser")

# 1. Innstillinger i sidebar eller øverst
col1, col2 = st.columns([2, 1])
with col1:
    user_input = st.text_input("Søk på selskap:", "Norconsult")
with col2:
    tidshorisont = st.selectbox(
        "Velg tidshorisont:",
        options=["1mo", "3mo", "6mo", "1y", "2y", "5y"],
        index=3  # Standard er 1y
    )

ticker_map = {"NORCONSULT": "NORCO.OL", "MULTICONSULT": "MULTI.OL", "AF GRUPPEN": "AFG.OL", "VEIDEKKE": "VEI.OL"}
search_term = user_input.strip()
yf_ticker = ticker_map.get(search_term.upper(), f"{search_term.upper()}.OL" if ".OL" not in search_term.upper() else search_term.upper())

if search_term:
    df = yf.Ticker(yf_ticker).history(period=tidshorisont)
    
    if not df.empty:
        df.index = pd.to_datetime(df.index).tz_localize(None).normalize()
        
        gn = GoogleNews(lang='no', country='NO')
        search = gn.search(search_term)
        viktige_ord = ["rapport", "oppkjøp", "resultat", "kontrakt", "kvartal", "q1", "q2", "q3", "q4", "emisjon"]
        
        fig = go.Figure()
        fig.add_trace(go.Scatter(x=df.index, y=df['Close'], mode='lines', name='Kurs', line=dict(color='#1f77b4', width=2)))

        display_news = []
        start_dato = df.index.min()

        for entry in search['entries']:
            if any(word in entry.title.lower() for word in viktige_ord):
                try:
                    dt = pd.to_datetime(entry.published).tz_localize(None).normalize()
                    
                    if dt >= start_dato:
                        # Finn nærmeste pris hvis nøyaktig dato mangler (helg)
                        idx = df.index.get_indexer([dt], method='nearest')[0]
                        price = df.iloc[idx]['Close']
                        actual_dt = df.index[idx]
                        
                        fig.add_trace(go.Scatter(
                            x=[actual_dt], y=[price], mode='markers',
                            marker=dict(size=12, color='red', line=dict(width=1, color='white')),
                            hovertext=entry.title, hoverinfo="text", showlegend=False
                        ))
                        fig.add_vline(x=actual_dt, line_width=1, line_dash="dot", line_color="rgba(150,150,150,0.3)")
                        display_news.append({"Dato": dt.date(), "Hendelse": entry.title, "Link": entry.link})
                except: continue

        fig.update_layout(
            height=500, template="plotly_dark",
            xaxis=dict(range=[df.index.min(), df.index.max()]),
            yaxis=dict(range=[df['Close'].min() * 0.95, df['Close'].max() * 1.05]),
            margin=dict(l=0, r=0, t=20, b=0)
        )

        st.plotly_chart(fig, use_container_width=True)

        if display_news:
            st.write(f"### Relevante hendelser siste {tidshorisont}")
            news_df = pd.DataFrame(display_news).sort_values(by="Dato", ascending=False)
            st.dataframe(
                news_df,
                column_config={"Link": st.column_config.LinkColumn("Åpne artikkel")},
                hide_index=True, use_container_width=True
            )
    else:
        st.error(f"Fant ikke data for {yf_ticker} med tidshorisont {tidshorisont}.")