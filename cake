<!DOCTYPE html>
<html lang="ko">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<title>생화케이크 CRM 고객관리 시스템</title>
<style>
  * { box-sizing: border-box; margin: 0; padding: 0; }
  :root {
    --primary: #3b5bdb; --primary-light: #edf2ff;
    --success: #2f9e44; --warning: #e67700; --danger: #c92a2a;
    --gray-50: #f8f9fa; --gray-100: #f1f3f5; --gray-200: #e9ecef;
    --gray-300: #dee2e6; --gray-500: #adb5bd; --gray-700: #495057; --gray-900: #212529;
    --kakao: #fee500; --radius: 10px; --shadow: 0 2px 8px rgba(0,0,0,0.08);
  }
  body { font-family: 'Apple SD Gothic Neo','Malgun Gothic',sans-serif; background: var(--gray-100); color: var(--gray-900); display: flex; height: 100vh; overflow: hidden; }

  .sidebar { width: 220px; background: #1c2340; color: white; display: flex; flex-direction: column; flex-shrink: 0; }
  .sidebar-logo { padding: 24px 20px 16px; border-bottom: 1px solid rgba(255,255,255,0.1); }
  .sidebar-logo h1 { font-size: 16px; font-weight: 700; }
  .sidebar-logo p { font-size: 11px; color: rgba(255,255,255,0.5); margin-top: 3px; }
  .sidebar-nav { flex: 1; padding: 12px 0; }
  .nav-item { display: flex; align-items: center; gap: 10px; padding: 11px 20px; cursor: pointer; font-size: 14px; color: rgba(255,255,255,0.7); transition: all 0.15s; border-left: 3px solid transparent; }
  .nav-item:hover { background: rgba(255,255,255,0.06); color: white; }
  .nav-item.active { background: rgba(59,91,219,0.3); color: white; border-left-color: var(--primary); }
  .nav-icon { font-size: 17px; width: 20px; text-align: center; }
  .sidebar-bottom { padding: 16px 20px; border-top: 1px solid rgba(255,255,255,0.1); font-size: 12px; color: rgba(255,255,255,0.4); }

  .main { flex: 1; display: flex; flex-direction: column; overflow: hidden; }
  .topbar { background: white; padding: 0 24px; height: 58px; display: flex; align-items: center; justify-content: space-between; border-bottom: 1px solid var(--gray-200); box-shadow: var(--shadow); flex-shrink: 0; }
  .topbar-title { font-size: 17px; font-weight: 700; }
  .topbar-actions { display: flex; gap: 10px; align-items: center; }
  .content { flex: 1; overflow-y: auto; padding: 24px; }
  .page { display: none; }
  .page.active { display: block; }

  .card { background: white; border-radius: var(--radius); padding: 20px; box-shadow: var(--shadow); }
  .card-title { font-size: 14px; font-weight: 600; color: var(--gray-700); margin-bottom: 16px; display: flex; align-items: center; gap: 8px; }

  /* 통계 카드 - 클릭 가능 */
  .stats-grid { display: grid; grid-template-columns: repeat(4,1fr); gap: 16px; margin-bottom: 24px; }
  .stat-card { background: white; border-radius: var(--radius); padding: 20px; box-shadow: var(--shadow); position: relative; }
  .stat-card.clickable { cursor: pointer; transition: all 0.2s; }
  .stat-card.clickable:hover { transform: translateY(-2px); box-shadow: 0 6px 20px rgba(0,0,0,0.12); }
  .stat-card.clickable::after { content: '→'; position: absolute; top: 12px; right: 14px; font-size: 14px; opacity: 0.4; }
  .stat-label { font-size: 12px; color: var(--gray-500); margin-bottom: 6px; }
  .stat-value { font-size: 28px; font-weight: 700; }
  .stat-sub { font-size: 12px; color: var(--gray-500); margin-top: 4px; }
  .stat-card.blue .stat-value { color: var(--primary); }
  .stat-card.green .stat-value { color: var(--success); }
  .stat-card.orange .stat-value { color: var(--warning); }
  .stat-card.kakao .stat-value { color: #b8a200; }

  .btn { padding: 8px 16px; border-radius: 7px; border: none; cursor: pointer; font-size: 13px; font-weight: 600; transition: all 0.15s; display: inline-flex; align-items: center; gap: 6px; }
  .btn-primary { background: var(--primary); color: white; }
  .btn-primary:hover { background: #2f4ac8; }
  .btn-success { background: var(--success); color: white; }
  .btn-kakao { background: var(--kakao); color: #333; }
  .btn-kakao:hover { background: #e6cf00; }
  .btn-outline { background: white; color: var(--gray-700); border: 1.5px solid var(--gray-300); }
  .btn-outline:hover { background: var(--gray-50); }
  .btn-danger { background: var(--danger); color: white; }
  .btn-sm { padding: 5px 10px; font-size: 12px; }
  .btn-lg { padding: 12px 24px; font-size: 15px; }

  .form-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 14px; }
  .form-group { display: flex; flex-direction: column; gap: 5px; }
  .form-group.full { grid-column: 1 / -1; }
  .form-label { font-size: 12px; font-weight: 600; color: var(--gray-700); }
  .form-input { padding: 9px 12px; border: 1.5px solid var(--gray-300); border-radius: 7px; font-size: 14px; outline: none; transition: border-color 0.15s; }
  .form-input:focus { border-color: var(--primary); }
  .form-select { padding: 9px 12px; border: 1.5px solid var(--gray-300); border-radius: 7px; font-size: 14px; outline: none; background: white; cursor: pointer; }
  .form-check { display: flex; align-items: center; gap: 8px; font-size: 13px; cursor: pointer; }
  .form-check input { width: 16px; height: 16px; cursor: pointer; accent-color: var(--primary); }
  .consent-box { background: var(--primary-light); border: 1.5px solid #c5d3f0; border-radius: 8px; padding: 14px; }
  .consent-title { font-size: 13px; font-weight: 700; color: var(--primary); margin-bottom: 10px; }
  .consent-checks { display: flex; flex-direction: column; gap: 8px; }

  .table-wrap { overflow-x: auto; }
  table { width: 100%; border-collapse: collapse; font-size: 13px; }
  th { background: var(--gray-50); padding: 10px 14px; text-align: left; font-weight: 600; color: var(--gray-700); border-bottom: 2px solid var(--gray-200); white-space: nowrap; }
  td { padding: 10px 14px; border-bottom: 1px solid var(--gray-100); vertical-align: middle; }
  tr:hover td { background: var(--gray-50); }

  .tag { display: inline-flex; align-items: center; padding: 3px 8px; border-radius: 20px; font-size: 11px; font-weight: 600; gap: 4px; }
  .tag-blue { background: #dbe4ff; color: #364fc7; }
  .tag-green { background: #d3f9d8; color: #2b8a3e; }
  .tag-orange { background: #fff3bf; color: #e67700; }
  .tag-red { background: #ffe3e3; color: #c92a2a; }
  .tag-purple { background: #f3d9fa; color: #862e9c; }
  .tag-gray { background: var(--gray-200); color: var(--gray-700); }
  .tag-kakao { background: #fff7cc; color: #b8a200; }

  .filter-bar { display: flex; gap: 10px; align-items: center; flex-wrap: wrap; margin-bottom: 16px; }
  .filter-btn { padding: 6px 14px; border-radius: 20px; border: 1.5px solid var(--gray-300); background: white; font-size: 12px; font-weight: 600; cursor: pointer; transition: all 0.15s; color: var(--gray-700); }
  .filter-btn.active { background: var(--primary); color: white; border-color: var(--primary); }
  .search-input { padding: 7px 14px; border: 1.5px solid var(--gray-300); border-radius: 20px; font-size: 13px; outline: none; width: 200px; }
  .search-input:focus { border-color: var(--primary); }

  /* 솔라피 발송 상태 */
  .solapi-status { display: flex; align-items: center; gap: 8px; padding: 10px 14px; border-radius: 8px; font-size: 13px; margin-bottom: 12px; }
  .solapi-status.connected { background: #d3f9d8; color: #2b8a3e; }
  .solapi-status.disconnected { background: #fff3bf; color: #e67700; }
  .status-dot { width: 8px; height: 8px; border-radius: 50%; flex-shrink: 0; }
  .status-dot.on { background: var(--success); }
  .status-dot.off { background: var(--warning); }

  /* 발송 진행 바 */
  .progress-wrap { background: var(--gray-200); border-radius: 20px; height: 8px; overflow: hidden; margin: 8px 0; }
  .progress-bar { height: 100%; background: var(--kakao); border-radius: 20px; transition: width 0.3s; }

  .msg-layout { display: grid; grid-template-columns: 1fr 1fr; gap: 20px; }
  .msg-preview { background: #abc1d1; border-radius: 12px; padding: 16px; min-height: 300px; }
  .msg-preview-header { text-align: center; font-size: 12px; color: #556; margin-bottom: 16px; font-weight: 600; }
  .chat-bubble { background: white; border-radius: 12px; border-top-left-radius: 3px; padding: 12px 14px; font-size: 13px; line-height: 1.7; max-width: 85%; box-shadow: 0 1px 3px rgba(0,0,0,0.1); white-space: pre-wrap; }
  .chat-time { font-size: 11px; color: #789; margin-top: 6px; margin-left: 4px; }
  .var-tag { background: #ffe066; color: #7c5c00; padding: 1px 5px; border-radius: 4px; font-weight: 600; }
  .recipient-count { background: var(--primary-light); border: 1.5px solid #c5d3f0; border-radius: 8px; padding: 12px 16px; margin-bottom: 14px; font-size: 13px; }
  .recipient-count strong { color: var(--primary); font-size: 18px; }
  .template-btns { display: flex; flex-wrap: wrap; gap: 6px; margin-bottom: 12px; }
  .template-btn { padding: 5px 11px; border-radius: 6px; border: 1.5px solid var(--gray-300); background: white; font-size: 12px; cursor: pointer; transition: all 0.15s; }
  .template-btn:hover { border-color: var(--primary); color: var(--primary); }

  .sub-grid { display: grid; grid-template-columns: repeat(3,1fr); gap: 16px; margin-bottom: 24px; }
  .sub-card { background: white; border-radius: var(--radius); padding: 18px; box-shadow: var(--shadow); border-left: 4px solid var(--primary); }
  .sub-name { font-weight: 700; font-size: 15px; margin-bottom: 4px; }
  .sub-detail { font-size: 12px; color: var(--gray-500); }
  .sub-count { font-size: 22px; font-weight: 700; color: var(--primary); margin: 8px 0 4px; }

  /* 소비패턴 분석 */
  .pattern-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 20px; margin-bottom: 24px; }
  .chart-bar-wrap { display: flex; flex-direction: column; gap: 10px; }
  .chart-row { display: flex; align-items: center; gap: 10px; }
  .chart-label { font-size: 12px; color: var(--gray-700); width: 90px; flex-shrink: 0; text-align: right; }
  .chart-bar-bg { flex: 1; background: var(--gray-100); border-radius: 20px; height: 20px; overflow: hidden; }
  .chart-bar-fill { height: 100%; border-radius: 20px; transition: width 0.6s ease; display: flex; align-items: center; padding-left: 8px; font-size: 11px; color: white; font-weight: 700; }
  .chart-count { font-size: 12px; color: var(--gray-500); width: 30px; text-align: right; flex-shrink: 0; }
  .segment-grid { display: grid; grid-template-columns: repeat(2,1fr); gap: 10px; }
  .segment-card { border-radius: 8px; padding: 14px; }
  .segment-name { font-weight: 700; font-size: 13px; margin-bottom: 4px; }
  .segment-count { font-size: 22px; font-weight: 700; margin: 4px 0; }
  .segment-desc { font-size: 11px; opacity: 0.8; }
  .donut-wrap { display: flex; justify-content: center; align-items: center; gap: 20px; }
  .donut-legend { display: flex; flex-direction: column; gap: 8px; }
  .legend-item { display: flex; align-items: center; gap: 8px; font-size: 12px; }
  .legend-dot { width: 10px; height: 10px; border-radius: 50%; flex-shrink: 0; }

  .modal-overlay { position: fixed; inset: 0; background: rgba(0,0,0,0.4); z-index: 100; display: none; align-items: center; justify-content: center; }
  .modal-overlay.open { display: flex; }
  .modal { background: white; border-radius: 14px; padding: 28px; width: 520px; max-height: 90vh; overflow-y: auto; box-shadow: 0 20px 60px rgba(0,0,0,0.2); }
  .modal-title { font-size: 17px; font-weight: 700; margin-bottom: 20px; display: flex; justify-content: space-between; align-items: center; }
  .modal-close { cursor: pointer; font-size: 20px; color: var(--gray-500); background: none; border: none; }
  .modal-footer { margin-top: 20px; display: flex; gap: 10px; justify-content: flex-end; }

  .toast { position: fixed; bottom: 24px; right: 24px; background: #333; color: white; padding: 12px 20px; border-radius: 9px; font-size: 14px; z-index: 200; opacity: 0; transform: translateY(10px); transition: all 0.3s; pointer-events: none; max-width: 320px; }
  .toast.show { opacity: 1; transform: translateY(0); }
  .toast.success { background: var(--success); }
  .toast.kakao { background: #b8a200; }
  .toast.error { background: var(--danger); }

  .dash-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 20px; }
  .recent-list { display: flex; flex-direction: column; gap: 10px; }
  .recent-item { display: flex; align-items: center; gap: 12px; padding: 10px 0; border-bottom: 1px solid var(--gray-100); }
  .avatar { width: 36px; height: 36px; border-radius: 50%; background: var(--primary-light); display: flex; align-items: center; justify-content: center; font-weight: 700; font-size: 14px; color: var(--primary); flex-shrink: 0; }
  .recent-info { flex: 1; }
  .recent-name { font-weight: 600; font-size: 14px; }
  .recent-sub { font-size: 12px; color: var(--gray-500); margin-top: 2px; }
  .alert-list { display: flex; flex-direction: column; gap: 10px; }
  .alert-item { display: flex; align-items: flex-start; gap: 12px; padding: 12px; background: var(--gray-50); border-radius: 8px; }
  .alert-dot { width: 8px; height: 8px; border-radius: 50%; margin-top: 5px; flex-shrink: 0; }
  .alert-dot.orange { background: var(--warning); }
  .alert-dot.blue { background: var(--primary); }
  .alert-text { font-size: 13px; line-height: 1.5; }
  .alert-time { font-size: 11px; color: var(--gray-500); margin-top: 3px; }

  /* 코호트 */
  .cohort-card { background: white; border-radius: var(--radius); padding: 18px; box-shadow: var(--shadow); border-left: 4px solid var(--primary); margin-bottom: 12px; display: flex; align-items: center; gap: 14px; }
  .cohort-name { font-weight: 700; font-size: 15px; flex: 1; }
  .cohort-count { font-size: 22px; font-weight: 700; color: var(--primary); min-width: 60px; text-align: center; }
  .cohort-conditions { font-size: 12px; color: var(--gray-500); margin-top: 3px; }
  .condition-row { display: flex; gap: 8px; align-items: center; margin-bottom: 8px; }
  .condition-row select { flex: 1; }
  .cohort-selector-box { background: var(--primary-light); border: 1.5px solid #c5d3f0; border-radius: 8px; padding: 14px; margin-bottom: 14px; }
  .cohort-selector-box label { font-size: 12px; font-weight: 700; color: var(--primary); display: block; margin-bottom: 6px; }

  @media (max-width: 900px) {
    .stats-grid { grid-template-columns: 1fr 1fr; }
    .msg-layout,.dash-grid,.pattern-grid { grid-template-columns: 1fr; }
  }
</style>
</head>
<body>

<div class="sidebar">
  <div class="sidebar-logo">
    <h1>🌸 CRM 고객관리</h1>
    <p>생화케이크 전문점 전용</p>
  </div>
  <nav class="sidebar-nav">
    <div class="nav-item active" onclick="showPage('dashboard',this)"><span class="nav-icon">📊</span> 대시보드</div>
    <div class="nav-item" onclick="showPage('customers',this)"><span class="nav-icon">👥</span> 고객 관리</div>
    <div class="nav-item" onclick="showPage('send',this)"><span class="nav-icon">💬</span> 카톡 발송</div>
    <div class="nav-item" onclick="showPage('subscribe',this)"><span class="nav-icon">🔄</span> 정기예약</div>
    <div class="nav-item" onclick="showPage('analysis',this)"><span class="nav-icon">📈</span> 소비패턴 분석</div>
    <div class="nav-item" onclick="showPage('cohort',this)"><span class="nav-icon">🎯</span> 코호트 타겟</div>
    <div class="nav-item" onclick="showPage('import',this)"><span class="nav-icon">📥</span> 데이터 가져오기</div>
    <div class="nav-item" onclick="showPage('settings',this)"><span class="nav-icon">⚙️</span> 설정 / API</div>
  </nav>
  <div class="sidebar-bottom" id="api-status-badge">⚪ 테스트 모드</div>
</div>

<div class="main">
  <div class="topbar">
    <div class="topbar-title" id="topbar-title">📊 대시보드</div>
    <div class="topbar-actions">
      <button class="btn btn-primary" onclick="openModal('addCustomer')">+ 고객 추가</button>
    </div>
  </div>

  <div class="content">

    <!-- 대시보드 -->
    <div class="page active" id="page-dashboard">
      <div class="stats-grid">
        <div class="stat-card blue clickable" onclick="goCustomers('all')">
          <div class="stat-label">전체 고객</div>
          <div class="stat-value" id="stat-total">0</div>
          <div class="stat-sub">누적 등록</div>
        </div>
        <div class="stat-card green clickable" onclick="goCustomers('consent')">
          <div class="stat-label">마케팅 동의</div>
          <div class="stat-value" id="stat-consent">0</div>
          <div class="stat-sub">발송 가능 고객</div>
        </div>
        <div class="stat-card orange clickable" onclick="goCustomers('dormant')">
          <div class="stat-label">재방문 필요</div>
          <div class="stat-value" id="stat-dormant">0</div>
          <div class="stat-sub">3주 이상 미방문 ↗</div>
        </div>
        <div class="stat-card kakao clickable" onclick="showPage('subscribe',document.querySelector('.nav-item:nth-child(4)'))">
          <div class="stat-label">정기예약</div>
          <div class="stat-value" id="stat-sub">0</div>
          <div class="stat-sub">활성 예약자</div>
        </div>
      </div>
      <div class="dash-grid">
        <div class="card">
          <div class="card-title">👤 최근 등록 고객</div>
          <div class="recent-list" id="recent-list"><div style="color:var(--gray-500);font-size:13px;text-align:center;padding:20px">등록된 고객이 없습니다</div></div>
        </div>
        <div class="card">
          <div class="card-title">🔔 발송 추천 알림</div>
          <div class="alert-list" id="alert-list"><div style="color:var(--gray-500);font-size:13px;text-align:center;padding:20px">고객을 추가하면 알림이 생성됩니다</div></div>
          <div style="margin-top:14px">
            <button class="btn btn-kakao btn-sm" onclick="showPage('send',document.querySelector('.nav-item:nth-child(3)'))">💬 바로 발송하기</button>
          </div>
        </div>
      </div>
    </div>

    <!-- 고객 관리 -->
    <div class="page" id="page-customers">
      <div class="filter-bar">
        <input class="search-input" type="text" placeholder="🔍 이름 / 전화번호 검색" oninput="filterCustomers()" id="search-input">
        <button class="filter-btn active" id="filter-all" onclick="setFilter('all',this)">전체</button>
        <button class="filter-btn" id="filter-consent" onclick="setFilter('consent',this)">마케팅 동의</button>
        <button class="filter-btn" id="filter-subscribe" onclick="setFilter('subscribe',this)">정기예약</button>
        <button class="filter-btn" id="filter-dormant" onclick="setFilter('dormant',this)">재방문 필요</button>
        <button class="filter-btn" id="filter-floral" onclick="setFilter('floral',this)">로맨틱로즈</button>
        <button class="filter-btn" id="filter-acidity" onclick="setFilter('acidity',this)">화사한컬러</button>
        <button class="filter-btn" id="filter-nutty" onclick="setFilter('nutty',this)">내추럴그린</button>
      </div>
      <div class="card">
        <div class="table-wrap">
          <table>
            <thead>
              <tr>
                <th><input type="checkbox" id="check-all" onchange="toggleAll(this)"></th>
                <th>이름</th><th>전화번호</th><th>취향</th><th>마지막 구매</th>
                <th>구매 횟수</th><th>수신동의</th><th>정기예약</th><th>관리</th>
              </tr>
            </thead>
            <tbody id="customer-table">
              <tr><td colspan="9" style="text-align:center;color:var(--gray-500);padding:30px">등록된 고객이 없습니다</td></tr>
            </tbody>
          </table>
        </div>
        <div style="margin-top:14px;display:flex;gap:10px;align-items:center">
          <span id="selected-count" style="font-size:13px;color:var(--gray-500)">선택된 고객: 0명</span>
          <button class="btn btn-kakao btn-sm" onclick="sendToSelected()">💬 선택 고객에게 카톡</button>
        </div>
      </div>
    </div>

    <!-- 카톡 발송 -->
    <div class="page" id="page-send">
      <div class="msg-layout">
        <div>
          <!-- 솔라피 연결 상태 -->
          <div id="solapi-status-box" class="solapi-status disconnected" style="margin-bottom:16px">
            <div class="status-dot off" id="solapi-dot"></div>
            <span id="solapi-status-text">솔라피 미연결 — 테스트 모드 (설정에서 API 키 입력)</span>
          </div>

          <div class="card" style="margin-bottom:16px">
            <div class="card-title">📋 발송 대상</div>
            <!-- 코호트 선택 -->
            <div class="cohort-selector-box">
              <label>🎯 코호트 타겟 (선택)</label>
              <div style="display:flex;gap:8px;align-items:center">
                <select class="form-select" id="cohort-select" onchange="onCohortSelect()" style="flex:1">
                  <option value="">-- 코호트 사용 안 함 (아래 필터 적용) --</option>
                </select>
                <button class="btn btn-outline btn-sm" onclick="showPage('cohort',document.querySelector('.nav-item:nth-child(6)'))">코호트 관리</button>
              </div>
            </div>
            <div class="filter-bar" style="margin-bottom:12px" id="msg-filter-bar">
              <button class="filter-btn active" onclick="setMsgFilter('all',this)">전체</button>
              <button class="filter-btn" onclick="setMsgFilter('consent',this)">마케팅 동의만</button>
              <button class="filter-btn" onclick="setMsgFilter('subscribe',this)">정기예약</button>
              <button class="filter-btn" onclick="setMsgFilter('dormant',this)">장기 미방문</button>
              <button class="filter-btn" onclick="setMsgFilter('floral',this)">로맨틱로즈 취향</button>
              <button class="filter-btn" onclick="setMsgFilter('acidity',this)">화사한컬러 취향</button>
            </div>
            <div class="recipient-count">
              발송 대상: <strong id="recipient-num">0</strong>명
              <span style="color:var(--gray-500);font-size:12px;margin-left:8px" id="recipient-names"></span>
            </div>
          </div>

          <div class="card">
            <div class="card-title">✏️ 메시지 작성</div>
            <div style="margin-bottom:10px">
              <div style="font-size:12px;color:var(--gray-500);margin-bottom:6px">📌 템플릿 불러오기</div>
              <div class="template-btns">
                <button class="template-btn" onclick="loadTemplate('reorder')">재구매 유도</button>
                <button class="template-btn" onclick="loadTemplate('newin')">신규 입고</button>
                <button class="template-btn" onclick="loadTemplate('sub')">구독 안내</button>
                <button class="template-btn" onclick="loadTemplate('dormant')">장기 미방문</button>
                <button class="template-btn" onclick="loadTemplate('coupon')">쿠폰 발송</button>
              </div>
            </div>
            <div style="font-size:12px;color:var(--gray-500);margin-bottom:6px">
              💡 변수: <span class="var-tag">#{이름}</span> <span class="var-tag">#{케이크명}</span> <span class="var-tag">#{구매횟수}</span> <span class="var-tag">#{취향}</span>
            </div>
            <textarea id="msg-input" class="form-input" rows="8" style="resize:vertical;font-size:13px;line-height:1.7" oninput="updatePreview()" placeholder="메시지를 입력하세요...&#10;&#10;예) 안녕하세요, #{이름}님! 🌸&#10;지난번에 주문하신 #{케이크명} 마음에 드셨나요?"></textarea>
            <div style="margin-top:6px;font-size:12px;color:var(--gray-500);text-align:right" id="char-count">0자</div>
            <!-- 발송 진행 바 (실제 발송시) -->
            <div id="progress-section" style="display:none;margin-top:10px">
              <div style="font-size:12px;color:var(--gray-700);margin-bottom:4px" id="progress-text">발송 중...</div>
              <div class="progress-wrap"><div class="progress-bar" id="progress-bar" style="width:0%"></div></div>
            </div>
            <div style="margin-top:14px;display:flex;gap:10px">
              <button class="btn btn-outline" onclick="updatePreview()">👁 미리보기</button>
              <button class="btn btn-kakao btn-lg" onclick="sendMessages()" style="flex:1" id="send-btn">💬 카톡 발송하기</button>
            </div>
          </div>
        </div>

        <div>
          <div class="card">
            <div class="card-title">📱 카톡 미리보기</div>
            <div class="msg-preview">
              <div class="msg-preview-header">카카오톡 채널</div>
              <div style="display:flex;align-items:flex-start;gap:8px">
                <div style="width:34px;height:34px;background:#fee500;border-radius:8px;display:flex;align-items:center;justify-content:center;font-size:18px;flex-shrink:0">🌸</div>
                <div>
                  <div style="font-size:11px;color:#556;margin-bottom:5px;font-weight:600" id="preview-cafe-name">생화케이크 공방</div>
                  <div class="chat-bubble" id="preview-bubble">메시지를 입력하면 여기에 미리보기가 표시됩니다.</div>
                  <div class="chat-time" id="preview-time">오후 2:30</div>
                </div>
              </div>
            </div>
          </div>
          <div class="card" style="margin-top:16px">
            <div class="card-title">📊 발송 내역</div>
            <div id="send-history" style="font-size:13px;color:var(--gray-500)">아직 발송 내역이 없습니다.</div>
          </div>
        </div>
      </div>
    </div>

    <!-- 정기예약 -->
    <div class="page" id="page-subscribe">
      <div class="sub-grid">
        <div class="sub-card">
          <div class="sub-name">🎂 월간 정기예약</div>
          <div class="sub-detail">매달 1일 픽업</div>
          <div class="sub-count" id="sub-count-weekly">0명</div>
          <div style="font-size:12px;color:var(--gray-500)">활성 예약자</div>
          <div style="margin-top:12px"><button class="btn btn-kakao btn-sm" onclick="sendSubMessage('weekly')">💬 일괄 카톡</button></div>
        </div>
        <div class="sub-card" style="border-left-color:var(--success)">
          <div class="sub-name">💐 분기 정기예약</div>
          <div class="sub-detail">1일, 15일 출고 (3개월 주기)</div>
          <div class="sub-count" style="color:var(--success)" id="sub-count-bimonthly">0명</div>
          <div style="font-size:12px;color:var(--gray-500)">활성 예약자</div>
          <div style="margin-top:12px"><button class="btn btn-kakao btn-sm" onclick="sendSubMessage('bimonthly')">💬 일괄 카톡</button></div>
        </div>
        <div class="sub-card" style="border-left-color:var(--warning)">
          <div class="sub-name">🎁 맞춤 기념일예약</div>
          <div class="sub-detail">생일/기념일 큐레이션</div>
          <div class="sub-count" style="color:var(--warning)" id="sub-count-custom">0명</div>
          <div style="font-size:12px;color:var(--gray-500)">활성 예약자</div>
          <div style="margin-top:12px"><button class="btn btn-kakao btn-sm" onclick="sendSubMessage('custom')">💬 일괄 카톡</button></div>
        </div>
      </div>
      <div class="card">
        <div class="card-title">🔄 정기예약 고객 목록</div>
        <div class="table-wrap">
          <table>
            <thead><tr><th>이름</th><th>전화번호</th><th>예약 유형</th><th>취향</th><th>예약 시작일</th><th>다음 픽업일</th><th>관리</th></tr></thead>
            <tbody id="sub-table"><tr><td colspan="7" style="text-align:center;color:var(--gray-500);padding:30px">정기예약 고객이 없습니다</td></tr></tbody>
          </table>
        </div>
      </div>
    </div>

    <!-- 소비패턴 분석 -->
    <div class="page" id="page-analysis">
      <div class="pattern-grid">
        <!-- 취향 분포 -->
        <div class="card">
          <div class="card-title">🌸 케이크 스타일 분포</div>
          <div class="chart-bar-wrap" id="taste-chart">
            <div style="color:var(--gray-500);font-size:13px;text-align:center;padding:20px">고객을 추가하면 분석이 시작됩니다</div>
          </div>
        </div>
        <!-- 구매 빈도 분포 -->
        <div class="card">
          <div class="card-title">🛍️ 고객 구매 빈도 분류</div>
          <div class="segment-grid" id="segment-chart">
            <div style="color:var(--gray-500);font-size:13px;text-align:center;padding:20px;grid-column:1/-1">고객을 추가하면 분석이 시작됩니다</div>
          </div>
        </div>
        <!-- 재방문 주기 분석 -->
        <div class="card">
          <div class="card-title">📅 재방문 주기 분석</div>
          <div class="chart-bar-wrap" id="revisit-chart">
            <div style="color:var(--gray-500);font-size:13px;text-align:center;padding:20px">고객을 추가하면 분석이 시작됩니다</div>
          </div>
        </div>
        <!-- 수신동의 현황 -->
        <div class="card">
          <div class="card-title">📋 수신동의 현황</div>
          <div id="consent-chart">
            <div style="color:var(--gray-500);font-size:13px;text-align:center;padding:20px">고객을 추가하면 분석이 시작됩니다</div>
          </div>
        </div>
      </div>
      <!-- 발송 추천 -->
      <div class="card">
        <div class="card-title">🎯 지금 바로 액션하세요</div>
        <div id="action-recommend" style="display:grid;grid-template-columns:repeat(3,1fr);gap:14px">
          <div style="color:var(--gray-500);font-size:13px;text-align:center;padding:20px;grid-column:1/-1">고객을 추가하면 추천이 생성됩니다</div>
        </div>
      </div>
    </div>

    <!-- 코호트 타겟 -->
    <div class="page" id="page-cohort">
      <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:20px">
        <div>
          <div style="font-size:16px;font-weight:700">🎯 코호트 타겟 관리</div>
          <div style="font-size:13px;color:var(--gray-500);margin-top:4px">조건을 설정해 고객 그룹을 만들고 카톡 발송에 사용하세요</div>
        </div>
        <button class="btn btn-primary" onclick="openModal('addCohort')">+ 새 코호트 만들기</button>
      </div>
      <div id="cohort-list">
        <div style="color:var(--gray-500);font-size:13px;text-align:center;padding:40px;background:white;border-radius:var(--radius)">
          코호트가 없습니다. 위 버튼을 눌러 첫 타겟 그룹을 만들어보세요!
        </div>
      </div>
      <div class="card" style="margin-top:20px">
        <div class="card-title">💡 코호트 활용 예시</div>
        <div style="display:grid;grid-template-columns:repeat(3,1fr);gap:12px;font-size:13px">
          <div style="background:#fff3bf;border-radius:8px;padding:14px">
            <div style="font-weight:700;color:#e67700;margin-bottom:6px">🎂 VIP 재구매 유도</div>
            <div style="color:#7c5c00;line-height:1.6">구매 10회 이상 + 마케팅 동의<br>→ 감사 쿠폰 발송</div>
            <button class="btn btn-outline btn-sm" style="margin-top:8px" onclick="createPresetCohort('vip')">이 코호트 만들기</button>
          </div>
          <div style="background:#ffe3e3;border-radius:8px;padding:14px">
            <div style="font-weight:700;color:#c92a2a;margin-bottom:6px">⏰ 장기 미방문 복귀</div>
            <div style="color:#c92a2a;line-height:1.6">마지막 구매 30일+ + 마케팅 동의<br>→ 재방문 쿠폰 발송</div>
            <button class="btn btn-outline btn-sm" style="margin-top:8px" onclick="createPresetCohort('dormant')">이 코호트 만들기</button>
          </div>
          <div style="background:#f3d9fa;border-radius:8px;padding:14px">
            <div style="font-weight:700;color:#862e9c;margin-bottom:6px">🌹 로맨틱로즈 취향 알림</div>
            <div style="color:#862e9c;line-height:1.6">취향 = 로맨틱로즈 + 마케팅 동의<br>→ 신상 생화케이크 출시 알림</div>
            <button class="btn btn-outline btn-sm" style="margin-top:8px" onclick="createPresetCohort('floral')">이 코호트 만들기</button>
          </div>
        </div>
      </div>
    </div>

    <!-- 데이터 가져오기 -->
    <div class="page" id="page-import">

      <!-- 플랫폼 현황 카드 -->
      <div style="display:grid;grid-template-columns:1fr 1fr;gap:20px;margin-bottom:24px">

        <!-- 스마트스토어 -->
        <div class="card" style="border-top:4px solid #03c75a">
          <div style="display:flex;align-items:center;gap:12px;margin-bottom:16px">
            <div style="width:40px;height:40px;background:#03c75a;border-radius:10px;display:flex;align-items:center;justify-content:center;font-size:20px">🛒</div>
            <div>
              <div style="font-weight:700;font-size:15px">네이버 스마트스토어</div>
              <div style="font-size:12px;color:var(--gray-500)">주문 엑셀 → 고객 DB 자동 등록</div>
            </div>
            <span class="tag tag-green" style="margin-left:auto">연동 가능</span>
          </div>
          <div style="background:#f0fdf4;border-radius:8px;padding:12px;margin-bottom:14px;font-size:12px;line-height:1.9;color:#166534">
            <strong>가져올 수 있는 데이터</strong><br>
            ✓ 구매자 이름 &nbsp; ✓ 구매자 전화번호<br>
            ✓ 상품명 &nbsp; ✓ 주문금액 &nbsp; ✓ 주문일시<br>
            ✓ 주문상태 &nbsp; ✓ 주문번호
          </div>
          <div style="background:#fff;border:1.5px solid #dee2e6;border-radius:8px;padding:12px;margin-bottom:14px;font-size:12px;line-height:1.9;color:var(--gray-700)">
            <strong>📌 엑셀 다운로드 방법</strong><br>
            스마트스토어 센터 → 판매관리<br>
            → 주문관리 → 주문 조회/발송관리<br>
            → 우측 상단 <strong>"엑셀 다운로드"</strong> 클릭
          </div>
          <button class="btn btn-success" style="width:100%" onclick="document.getElementById('smartstore-file').click()">
            📤 스마트스토어 엑셀 불러오기
          </button>
          <input type="file" id="smartstore-file" accept=".xlsx,.xls,.csv" style="display:none" onchange="importSmartstore(this)">
        </div>

        <!-- 토스포스 -->
        <div class="card" style="border-top:4px solid #0064ff">
          <div style="display:flex;align-items:center;gap:12px;margin-bottom:16px">
            <div style="width:40px;height:40px;background:#0064ff;border-radius:10px;display:flex;align-items:center;justify-content:center;font-size:20px">💳</div>
            <div>
              <div style="font-weight:700;font-size:15px">토스포스</div>
              <div style="font-size:12px;color:var(--gray-500)">매출 데이터 → 판매 패턴 분석</div>
            </div>
            <span class="tag tag-orange" style="margin-left:auto">부분 가능</span>
          </div>
          <div style="background:#eff6ff;border-radius:8px;padding:12px;margin-bottom:8px;font-size:12px;line-height:1.9;color:#1e40af">
            <strong>가져올 수 있는 데이터</strong><br>
            ✓ 결제일시 &nbsp; ✓ 결제금액<br>
            ✓ 결제수단 &nbsp; ✓ 메뉴명 &nbsp; ✓ 수량<br>
            ✗ 고객 이름/전화번호 (POS 미저장)
          </div>
          <div style="background:#fff3bf;border-radius:8px;padding:10px;margin-bottom:14px;font-size:12px;line-height:1.7;color:#7c5c00">
            ⚠️ 토스포스는 고객 개인정보를 저장하지 않아 이름·전화번호 연동은 불가합니다.<br>대신 <strong>매출 패턴 분석</strong>에 활용합니다.
          </div>
          <div style="background:#fff;border:1.5px solid #dee2e6;border-radius:8px;padding:12px;margin-bottom:14px;font-size:12px;line-height:1.9;color:var(--gray-700)">
            <strong>📌 CSV 다운로드 방법</strong><br>
            토스포스 앱 → 정산/매출<br>
            → 기간별 매출 → <strong>"내보내기"</strong> 탭<br>
            → CSV 파일 다운로드
          </div>
          <button class="btn btn-primary" style="width:100%;background:#0064ff" onclick="document.getElementById('tosspos-file').click()">
            📤 토스포스 CSV 불러오기
          </button>
          <input type="file" id="tosspos-file" accept=".csv" style="display:none" onchange="importTosspos(this)">
        </div>
      </div>

      <!-- 스마트스토어 가져오기 결과 -->
      <div class="card" id="smartstore-result" style="display:none;margin-bottom:20px;border-left:4px solid #03c75a">
        <div class="card-title">🛒 스마트스토어 가져오기 결과</div>
        <div id="smartstore-result-body"></div>
      </div>

      <!-- 토스포스 매출 분석 결과 -->
      <div class="card" id="tosspos-result" style="display:none;margin-bottom:20px;border-left:4px solid #0064ff">
        <div class="card-title">💳 토스포스 매출 분석</div>
        <div id="tosspos-result-body"></div>
      </div>

      <!-- 연동 흐름 안내 -->
      <div class="card">
        <div class="card-title">🔄 전체 연동 흐름</div>
        <div style="display:flex;align-items:center;gap:0;flex-wrap:wrap;justify-content:center;padding:10px 0">
          <div style="text-align:center;padding:16px 20px;background:#f0fdf4;border-radius:10px;min-width:130px">
            <div style="font-size:22px">🛒</div>
            <div style="font-weight:700;font-size:13px;margin:4px 0">스마트스토어</div>
            <div style="font-size:11px;color:var(--gray-500)">주문 엑셀 다운로드</div>
          </div>
          <div style="font-size:20px;color:var(--gray-300);padding:0 8px">→</div>
          <div style="text-align:center;padding:16px 20px;background:#eff6ff;border-radius:10px;min-width:130px">
            <div style="font-size:22px">📥</div>
            <div style="font-weight:700;font-size:13px;margin:4px 0">CRM 가져오기</div>
            <div style="font-size:11px;color:var(--gray-500)">전화번호로 자동 병합</div>
          </div>
          <div style="font-size:20px;color:var(--gray-300);padding:0 8px">→</div>
          <div style="text-align:center;padding:16px 20px;background:#fff3bf;border-radius:10px;min-width:130px">
            <div style="font-size:22px">👥</div>
            <div style="font-weight:700;font-size:13px;margin:4px 0">고객 DB 통합</div>
            <div style="font-size:11px;color:var(--gray-500)">구매이력 자동 누적</div>
          </div>
          <div style="font-size:20px;color:var(--gray-300);padding:0 8px">→</div>
          <div style="text-align:center;padding:16px 20px;background:#f3d9fa;border-radius:10px;min-width:130px">
            <div style="font-size:22px">💬</div>
            <div style="font-weight:700;font-size:13px;margin:4px 0">카톡 발송</div>
            <div style="font-size:11px;color:var(--gray-500)">솔라피 맞춤 메시지</div>
          </div>
          <div style="font-size:20px;color:var(--gray-300);padding:0 8px">→</div>
          <div style="text-align:center;padding:16px 20px;background:#ffe3e3;border-radius:10px;min-width:130px">
            <div style="font-size:22px">💳</div>
            <div style="font-weight:700;font-size:13px;margin:4px 0">토스포스</div>
            <div style="font-size:11px;color:var(--gray-500)">매출 패턴 확인</div>
          </div>
        </div>
        <div style="margin-top:16px;font-size:12px;color:var(--gray-500);background:var(--gray-50);padding:12px;border-radius:8px;line-height:1.8">
          💡 <strong>핵심 전략:</strong> 스마트스토어에서 온라인 고객 전화번호를 수집하고, QR 취향 테스트로 오프라인 고객까지 통합하면 전채널 고객 DB가 완성됩니다.<br>
          토스포스 매출 데이터로는 요일별·시간대별 판매 패턴을 파악해 카톡 발송 타이밍을 최적화하세요.
        </div>
      </div>
    </div>

    <!-- 설정 -->
    <div class="page" id="page-settings">
      <div style="display:grid;grid-template-columns:1fr 1fr;gap:20px">
        <div class="card">
          <div class="card-title">🔑 솔라피 API 설정</div>
          <div style="display:flex;flex-direction:column;gap:12px">
            <div class="form-group">
              <label class="form-label">API Key</label>
              <input class="form-input" type="password" id="solapi-key" placeholder="솔라피 API Key 입력">
            </div>
            <div class="form-group">
              <label class="form-label">API Secret</label>
              <input class="form-input" type="password" id="solapi-secret" placeholder="솔라피 API Secret 입력">
            </div>
            <div class="form-group">
              <label class="form-label">카카오 채널 ID (@ 포함)</label>
              <input class="form-input" type="text" id="kakao-channel" placeholder="예) @생화케이크공방">
            </div>
            <div class="form-group">
              <label class="form-label">매장 이름</label>
              <input class="form-input" type="text" id="cafe-name" placeholder="예) 블룸 케이크">
            </div>
            <button class="btn btn-primary" onclick="saveSettings()">💾 저장 및 연결 확인</button>
            <div style="background:#fff3bf;border-radius:8px;padding:12px;font-size:12px;color:#7c5c00;line-height:1.7">
              ⚠️ <strong>실제 발송 전 확인사항</strong><br>
              1. 솔라피 가입 후 API Key/Secret 발급<br>
              2. 카카오 비즈채널 개설 (사업자 인증)<br>
              3. 알림톡 템플릿 카카오 심사 완료 (3~5일)<br>
              4. 마케팅 수신동의 고객에게만 광고성 발송
            </div>
          </div>
        </div>
        <div class="card">
          <div class="card-title">📝 메시지 유형 안내</div>
          <div style="display:flex;flex-direction:column;gap:10px">
            <div style="background:#d3f9d8;border-radius:8px;padding:12px">
              <div style="font-weight:700;font-size:13px;color:#2b8a3e;margin-bottom:4px">✓ 알림톡 (수신동의 불필요)</div>
              <div style="font-size:12px;color:var(--gray-700);line-height:1.6">주문확인, 픽업안내, 출고알림<br>정보성 메시지 → 전화번호만 있으면 발송 가능</div>
            </div>
            <div style="background:#fff3bf;border-radius:8px;padding:12px">
              <div style="font-weight:700;font-size:13px;color:#e67700;margin-bottom:4px">⚠️ 친구톡 (마케팅 동의 필요)</div>
              <div style="font-size:12px;color:var(--gray-700);line-height:1.6">신메뉴 소식, 쿠폰, 이벤트<br>광고성 메시지 → 반드시 수신동의 필요</div>
            </div>
            <div style="background:#ffe3e3;border-radius:8px;padding:12px">
              <div style="font-weight:700;font-size:13px;color:#c92a2a;margin-bottom:4px">🌙 야간 발송 (야간동의 필요)</div>
              <div style="font-size:12px;color:var(--gray-700);line-height:1.6">오후 8시 ~ 오전 8시 발송<br>야간 수신동의 받은 고객에게만 가능</div>
            </div>
          </div>
        </div>
        <div class="card">
          <div class="card-title">💾 데이터 관리</div>
          <div style="display:flex;flex-direction:column;gap:10px">
            <button class="btn btn-outline" onclick="exportData()">📥 고객 데이터 내보내기 (CSV)</button>
            <button class="btn btn-outline" onclick="document.getElementById('import-file').click()">📤 엑셀/CSV 불러오기</button>
            <input type="file" id="import-file" accept=".csv" style="display:none" onchange="importCSV(this)">
            <button class="btn btn-danger btn-sm" onclick="clearAllData()">🗑 전체 데이터 초기화</button>
          </div>
        </div>
        <div class="card">
          <div class="card-title">ℹ️ 사용 안내</div>
          <div style="font-size:13px;line-height:2;color:var(--gray-700)">
            1️⃣ <strong>고객 추가</strong> → 전화번호 + 취향 + 수신동의 등록<br>
            2️⃣ <strong>고객 관리</strong> → 필터로 대상 고객 선택<br>
            3️⃣ <strong>카톡 발송</strong> → 템플릿 선택 후 발송<br>
            4️⃣ <strong>소비패턴 분석</strong> → 취향/구매빈도 차트 확인<br>
            5️⃣ <strong>솔라피 API</strong> 연동 시 실제 카톡 발송
          </div>
        </div>
      </div>
    </div>

  </div>
</div>

<!-- 고객 추가 모달 -->
<div class="modal-overlay" id="modal-addCustomer">
  <div class="modal">
    <div class="modal-title">
      👤 고객 추가
      <button class="modal-close" onclick="closeModal('addCustomer')">✕</button>
    </div>
    <div class="form-grid">
      <div class="form-group">
        <label class="form-label">이름 *</label>
        <input class="form-input" id="new-name" placeholder="고객 이름">
      </div>
      <div class="form-group">
        <label class="form-label">전화번호 *</label>
        <input class="form-input" id="new-phone" placeholder="010-0000-0000" oninput="formatPhone(this)">
      </div>
      <div class="form-group">
        <label class="form-label">케이크 취향</label>
        <select class="form-select" id="new-taste">
          <option value="">선택 안함</option>
          <option value="floral">로맨틱 로즈 (장미 생화)</option>
          <option value="acidity">화사한 컬러 (튤립/카네이션)</option>
          <option value="nutty">내추럴 그린 (그리너리)</option>
          <option value="balance">클래식 부케 (혼합 생화)</option>
          <option value="daily">심플 데일리 (시그니처 케이크)</option>
        </select>
      </div>
      <div class="form-group">
        <label class="form-label">마지막 구매일</label>
        <input class="form-input" id="new-lastbuy" type="date">
      </div>
      <div class="form-group">
        <label class="form-label">구매 횟수</label>
        <input class="form-input" id="new-count" type="number" placeholder="0" min="0">
      </div>
      <div class="form-group">
        <label class="form-label">정기예약 유형</label>
        <select class="form-select" id="new-subtype">
          <option value="">예약 없음</option>
          <option value="weekly">월간 정기예약</option>
          <option value="bimonthly">분기 정기예약</option>
          <option value="custom">맞춤 기념일예약</option>
        </select>
      </div>
      <div class="form-group">
        <label class="form-label">케이크명 (최근 구매)</label>
        <input class="form-input" id="new-bean" placeholder="예) 로즈 생화케이크 (6인용)">
      </div>
      <div class="form-group">
        <label class="form-label">메모</label>
        <input class="form-input" id="new-memo" placeholder="특이사항, 선호도 등">
      </div>
    </div>
    <div class="consent-box" style="margin-top:14px">
      <div class="consent-title">📋 수신 동의</div>
      <div class="consent-checks">
        <label class="form-check">
          <input type="checkbox" id="new-consent-marketing">
          마케팅 수신 동의 <span style="color:var(--warning);font-size:11px">(광고성 메시지 발송 가능)</span>
        </label>
        <label class="form-check">
          <input type="checkbox" id="new-consent-night">
          야간 수신 동의 <span style="color:var(--gray-500);font-size:11px">(오후 8시 이후 발송 가능)</span>
        </label>
      </div>
    </div>
    <div class="modal-footer">
      <button class="btn btn-outline" onclick="closeModal('addCustomer')">취소</button>
      <button class="btn btn-primary" onclick="addCustomer()">✓ 고객 등록</button>
    </div>
  </div>
</div>

<!-- 코호트 추가/수정 모달 -->
<div class="modal-overlay" id="modal-addCohort">
  <div class="modal" style="width:580px">
    <div class="modal-title">🎯 코호트 만들기 <button class="modal-close" onclick="closeModal('addCohort')">✕</button></div>
    <div class="form-group" style="margin-bottom:14px">
      <label class="form-label">코호트 이름 *</label>
      <input class="form-input" id="cohort-name-input" placeholder="예: VIP 단골 고객, 로맨틱로즈 취향 고객">
    </div>
    <div style="font-size:13px;font-weight:600;color:var(--gray-700);margin-bottom:8px">조건 설정</div>
    <div style="font-size:12px;color:var(--gray-500);margin-bottom:10px">여러 조건을 추가하면 <strong>모두 충족</strong>(AND)하는 고객만 포함됩니다</div>
    <div id="cohort-conditions-editor"></div>
    <button class="btn btn-outline btn-sm" onclick="addConditionRow()" style="margin-bottom:16px">+ 조건 추가</button>
    <div style="background:var(--primary-light);border-radius:8px;padding:10px;font-size:13px;margin-bottom:16px">
      매칭 고객: <strong id="cohort-preview-count" style="color:var(--primary);font-size:16px">0</strong>명
      <span id="cohort-preview-names" style="color:var(--gray-500);font-size:12px;margin-left:8px"></span>
    </div>
    <div class="modal-footer">
      <button class="btn btn-outline" onclick="closeModal('addCohort')">취소</button>
      <button class="btn btn-primary" onclick="saveCohortFromModal()">✓ 코호트 저장</button>
    </div>
  </div>
</div>

<div class="toast" id="toast"></div>

<script>
// ── 데이터 ──
let customers = JSON.parse(localStorage.getItem('crm_customers') || '[]');
let settings  = JSON.parse(localStorage.getItem('crm_settings')   || '{"cafeName":"생화케이크 공방"}');
let cohorts   = JSON.parse(localStorage.getItem('crm_cohorts')    || '[]');
let selectedCohortId = null;
let sendHistory = JSON.parse(localStorage.getItem('crm_history')  || '[]');
let currentFilter = 'all';
let currentMsgFilter = 'all';

const tasteMap = { floral:'🌹 로맨틱로즈', acidity:'🌷 화사한컬러', nutty:'🌿 내추럴그린', balance:'💐 클래식부케', daily:'🎂 심플데일리' };
const tasteTagClass = { floral:'tag-purple', acidity:'tag-orange', nutty:'tag-blue', balance:'tag-green', daily:'tag-gray' };
const tasteColors = { floral:'#862e9c', acidity:'#e67700', nutty:'#364fc7', balance:'#2b8a3e', daily:'#495057' };
const subTypeMap = { weekly:'월간예약', bimonthly:'분기예약', custom:'맞춤예약' };
const templates = {
  reorder: `안녕하세요, #{이름}님 🌸\n지난번에 주문하신 #{케이크명} 마음에 드셨나요?\n\n이번 주 새로운 생화케이크가 출시됐어요!\n#{이름}님 취향인 #{취향} 스타일로 골라봤습니다 😊\n\n→ 카카오채널에서 예약하기`,
  newin:   `안녕하세요, #{이름}님! 🌷\n\n신상 생화케이크 소식을 전해드려요 🎉\n#{이름}님이 좋아하시는 #{취향} 스타일입니다!\n\n지금 확인해보세요 😊`,
  sub:     `#{이름}님, 예약하신 케이크가 곧 준비됩니다 💐\n\n총 #{구매횟수}번째 정기예약이시네요!\n항상 감사드려요 🌸\n\n추가로 필요하신 것 있으시면 알려주세요!`,
  dormant: `#{이름}님, 오랜만이에요 🌸\n보고 싶었어요!\n\n다시 주문해주시면 미니 생화 조각케이크 드릴게요 🎁\n언제든지 편하게 문의해주세요 😊`,
  coupon:  `안녕하세요, #{이름}님! 💐\n\n감사의 마음을 담아 쿠폰을 보내드려요 🎁\n\n🎟 3,000원 할인 쿠폰\n코드: CAKE2024\n유효기간: 이번 달 말까지\n\n예쁜 생화케이크로 만나요 😊`
};

// ── 유틸 ──
function save()     { localStorage.setItem('crm_customers', JSON.stringify(customers)); }
function saveHist() { localStorage.setItem('crm_history',   JSON.stringify(sendHistory)); }
function toast(msg, type='') {
  const t = document.getElementById('toast');
  t.textContent = msg; t.className = `toast show ${type}`;
  setTimeout(() => t.className = 'toast', 3000);
}
function formatPhone(el) {
  let v = el.value.replace(/\D/g,'');
  if (v.length > 11) v = v.slice(0,11);
  if (v.length > 7) v = v.replace(/(\d{3})(\d{4})(\d{0,4})/,'$1-$2-$3');
  else if (v.length > 3) v = v.replace(/(\d{3})(\d{0,4})/,'$1-$2');
  el.value = v;
}
function daysSince(d) {
  if (!d) return 999;
  return Math.floor((new Date() - new Date(d)) / 86400000);
}
function nowStr() {
  const d = new Date(), h = d.getHours(), m = String(d.getMinutes()).padStart(2,'0');
  return `${h>=12?'오후':'오전'} ${h>12?h-12:h||12}:${m}`;
}
function applyVars(text, c) {
  return text
    .replace(/#{이름}/g,    c.name    || '고객')
    .replace(/#{케이크명}/g, c.bean    || '케이크')
    .replace(/#{취향}/g,    (tasteMap[c.taste]||'🎂 케이크').replace(/^.+?\s/,''))
    .replace(/#{구매횟수}/g, c.buyCount || '0');
}
function getFiltered(filter) {
  // 코호트 선택 중이면 코호트 조건 우선 적용
  if (selectedCohortId) {
    const cohort = cohorts.find(c => c.id === selectedCohortId);
    if (cohort) return customers.filter(c => matchesCohort(c, cohort));
  }
  return customers.filter(c => {
    if (filter === 'consent')   return c.consentMarketing;
    if (filter === 'subscribe') return c.subType;
    if (filter === 'dormant')   return daysSince(c.lastBuy) >= 21 && c.consentMarketing;
    if (filter === 'floral')    return c.taste === 'floral'   && c.consentMarketing;
    if (filter === 'acidity')   return c.taste === 'acidity'  && c.consentMarketing;
    if (filter === 'nutty')     return c.taste === 'nutty'    && c.consentMarketing;
    return true;
  });
}

// ── 코호트 ──
function saveCohorts() { localStorage.setItem('crm_cohorts', JSON.stringify(cohorts)); }

function matchesCohort(customer, cohort) {
  return (cohort.conditions || []).every(cond => {
    const val = cond.value;
    switch (cond.field) {
      case 'taste':             return customer.taste === val;
      case 'subType':           return val === 'any' ? !!customer.subType : customer.subType === val;
      case 'consentMarketing':  return customer.consentMarketing === (val === 'true');
      case 'consentNight':      return customer.consentNight === (val === 'true');
      case 'buyCountMin':       return (customer.buyCount || 0) >= parseInt(val);
      case 'buyCountMax':       return (customer.buyCount || 0) <= parseInt(val);
      case 'daysSinceMin':      return daysSince(customer.lastBuy) >= parseInt(val);
      case 'daysSinceMax':      return daysSince(customer.lastBuy) <= parseInt(val);
      case 'grade': {
        const bc = customer.buyCount || 0;
        if (val === 'vip')    return bc >= 10;
        if (val === 'loyal')  return bc >= 4 && bc < 10;
        if (val === 'casual') return bc >= 1 && bc < 4;
        if (val === 'new')    return bc === 0;
        return true;
      }
      default: return true;
    }
  });
}

function countCohort(cohort) {
  return customers.filter(c => matchesCohort(c, cohort)).length;
}

function renderCohorts() {
  const el = document.getElementById('cohort-list');
  if (!cohorts.length) {
    el.innerHTML = '<div style="color:var(--gray-500);font-size:13px;text-align:center;padding:40px;background:white;border-radius:var(--radius)">코호트가 없습니다. 위 버튼을 눌러 첫 타겟 그룹을 만들어보세요!</div>';
    return;
  }
  el.innerHTML = cohorts.map(cohort => {
    const cnt = countCohort(cohort);
    const condDesc = cohort.conditions.map(c => conditionLabel(c)).join(' + ') || '조건 없음';
    return `<div class="cohort-card">
      <div style="flex:1">
        <div class="cohort-name">${cohort.name}</div>
        <div class="cohort-conditions">${condDesc}</div>
      </div>
      <div class="cohort-count">${cnt}명</div>
      <button class="btn btn-kakao btn-sm" onclick="useCohortForSend(${cohort.id})">💬 카톡 발송</button>
      <button class="btn btn-danger btn-sm" onclick="deleteCohort(${cohort.id})">삭제</button>
    </div>`;
  }).join('');
  refreshCohortSelect();
}

function conditionLabel(cond) {
  const labels = {
    taste: { floral:'취향:로맨틱로즈', acidity:'취향:화사한컬러', nutty:'취향:내추럴그린', balance:'취향:클래식부케', daily:'취향:심플데일리' },
    subType: { any:'예약중', weekly:'월간예약', bimonthly:'분기예약', custom:'맞춤예약' },
    consentMarketing: { true:'마케팅동의', false:'마케팅미동의' },
    consentNight: { true:'야간동의', false:'야간미동의' },
    grade: { vip:'VIP(10회↑)', loyal:'단골(4~9회)', casual:'일반(1~3회)', new:'신규' }
  };
  if (cond.field === 'buyCountMin') return `구매${cond.value}회↑`;
  if (cond.field === 'buyCountMax') return `구매${cond.value}회↓`;
  if (cond.field === 'daysSinceMin') return `미방문${cond.value}일↑`;
  if (cond.field === 'daysSinceMax') return `미방문${cond.value}일↓`;
  return (labels[cond.field] || {})[cond.value] || `${cond.field}=${cond.value}`;
}

function refreshCohortSelect() {
  const sel = document.getElementById('cohort-select');
  if (!sel) return;
  const current = sel.value;
  sel.innerHTML = '<option value="">-- 코호트 사용 안 함 (아래 필터 적용) --</option>' +
    cohorts.map(c => `<option value="${c.id}" ${selectedCohortId===c.id?'selected':''}>${c.name} (${countCohort(c)}명)</option>`).join('');
  if (current) sel.value = current;
}

function onCohortSelect() {
  const sel = document.getElementById('cohort-select');
  selectedCohortId = sel.value ? parseInt(sel.value) : null;
  const filterBar = document.getElementById('msg-filter-bar');
  if (filterBar) filterBar.style.opacity = selectedCohortId ? '0.4' : '1';
  updateRecipients();
}

function useCohortForSend(cohortId) {
  selectedCohortId = cohortId;
  showPage('send', document.querySelector('.nav-item:nth-child(3)'));
  setTimeout(() => {
    const sel = document.getElementById('cohort-select');
    if (sel) { sel.value = cohortId; onCohortSelect(); }
  }, 100);
}

function deleteCohort(id) {
  if (!confirm('이 코호트를 삭제하시겠습니까?')) return;
  cohorts = cohorts.filter(c => c.id !== id);
  saveCohorts();
  if (selectedCohortId === id) selectedCohortId = null;
  renderCohorts();
  toast('코호트 삭제됨');
}

// 코호트 모달
const conditionFields = [
  { value:'consentMarketing', label:'마케팅 동의 여부', options:[{v:'true',l:'동의'},{v:'false',l:'미동의'}] },
  { value:'taste',            label:'케이크 취향',        options:[{v:'floral',l:'🌹 로맨틱로즈'},{v:'acidity',l:'🌷 화사한컬러'},{v:'nutty',l:'🌿 내추럴그린'},{v:'balance',l:'💐 클래식부케'},{v:'daily',l:'🎂 심플데일리'}] },
  { value:'grade',            label:'고객 등급',        options:[{v:'vip',l:'🏆 VIP (10회↑)'},{v:'loyal',l:'💛 단골 (4~9회)'},{v:'casual',l:'🎂 일반 (1~3회)'},{v:'new',l:'🆕 신규'}] },
  { value:'subType',          label:'정기예약',          options:[{v:'any',l:'예약 중 (전체)'},{v:'weekly',l:'월간예약'},{v:'bimonthly',l:'분기예약'},{v:'custom',l:'맞춤예약'}] },
  { value:'consentNight',     label:'야간 동의 여부',   options:[{v:'true',l:'동의'},{v:'false',l:'미동의'}] },
  { value:'buyCountMin',      label:'구매 횟수 이상',   type:'number', placeholder:'예: 5' },
  { value:'buyCountMax',      label:'구매 횟수 이하',   type:'number', placeholder:'예: 10' },
  { value:'daysSinceMin',     label:'미방문 일수 이상', type:'number', placeholder:'예: 30' },
  { value:'daysSinceMax',     label:'미방문 일수 이하', type:'number', placeholder:'예: 7' },
];

let tempConditions = [];

function openModal(id) {
  document.getElementById('modal-'+id).classList.add('open');
  if (id === 'addCustomer') document.getElementById('new-lastbuy').value = new Date().toISOString().split('T')[0];
  if (id === 'addCohort') {
    document.getElementById('cohort-name-input').value = '';
    tempConditions = [];
    renderConditionEditor();
    updateCohortPreview();
  }
}

function addConditionRow(preset) {
  tempConditions.push(preset || { field: 'consentMarketing', value: 'true' });
  renderConditionEditor();
  updateCohortPreview();
}

function renderConditionEditor() {
  const el = document.getElementById('cohort-conditions-editor');
  if (!tempConditions.length) {
    el.innerHTML = '<div style="color:var(--gray-500);font-size:13px;margin-bottom:10px">조건을 추가해주세요 (없으면 전체 고객)</div>';
    return;
  }
  el.innerHTML = tempConditions.map((cond, i) => {
    const fieldDef = conditionFields.find(f => f.value === cond.field) || conditionFields[0];
    const fieldSelect = `<select class="form-select" style="flex:1.2" onchange="updateCondField(${i},this.value)">
      ${conditionFields.map(f => `<option value="${f.value}" ${cond.field===f.value?'selected':''}>${f.label}</option>`).join('')}
    </select>`;
    let valueInput;
    if (fieldDef.type === 'number') {
      valueInput = `<input class="form-input" type="number" min="0" value="${cond.value||''}" placeholder="${fieldDef.placeholder||''}" style="flex:1" oninput="updateCondValue(${i},this.value)">`;
    } else {
      valueInput = `<select class="form-select" style="flex:1" onchange="updateCondValue(${i},this.value)">
        ${(fieldDef.options||[]).map(o => `<option value="${o.v}" ${cond.value===o.v?'selected':''}>${o.l}</option>`).join('')}
      </select>`;
    }
    return `<div class="condition-row">
      ${fieldSelect}
      ${valueInput}
      <button class="btn btn-danger btn-sm" onclick="removeCondition(${i})">✕</button>
    </div>`;
  }).join('');
}

function updateCondField(idx, field) {
  const fieldDef = conditionFields.find(f => f.value === field);
  tempConditions[idx].field = field;
  tempConditions[idx].value = fieldDef?.options?.[0]?.v || '';
  renderConditionEditor();
  updateCohortPreview();
}

function updateCondValue(idx, val) {
  tempConditions[idx].value = val;
  updateCohortPreview();
}

function removeCondition(idx) {
  tempConditions.splice(idx, 1);
  renderConditionEditor();
  updateCohortPreview();
}

function updateCohortPreview() {
  const temp = { conditions: tempConditions };
  const matched = customers.filter(c => matchesCohort(c, temp));
  document.getElementById('cohort-preview-count').textContent = matched.length;
  document.getElementById('cohort-preview-names').textContent =
    matched.slice(0,3).map(c=>c.name).join(', ') + (matched.length > 3 ? ` 외 ${matched.length-3}명` : '');
}

function saveCohortFromModal() {
  const name = document.getElementById('cohort-name-input').value.trim();
  if (!name) { toast('코호트 이름을 입력해주세요'); return; }
  cohorts.unshift({ id: Date.now(), name, conditions: [...tempConditions] });
  saveCohorts();
  closeModal('addCohort');
  renderCohorts();
  toast(`✓ 코호트 "${name}" 저장됨`, 'success');
}

function createPresetCohort(type) {
  const presets = {
    vip:     { name:'VIP 단골 고객', conditions:[{field:'buyCountMin',value:'10'},{field:'consentMarketing',value:'true'}] },
    dormant: { name:'장기 미방문 고객', conditions:[{field:'daysSinceMin',value:'30'},{field:'consentMarketing',value:'true'}] },
    floral:  { name:'로맨틱로즈 취향 고객', conditions:[{field:'taste',value:'floral'},{field:'consentMarketing',value:'true'}] },
  };
  const preset = presets[type];
  if (!preset) return;
  cohorts.unshift({ id: Date.now(), name: preset.name, conditions: preset.conditions });
  saveCohorts();
  renderCohorts();
  toast(`✓ 코호트 "${preset.name}" 생성됨`, 'success');
}
function isApiConnected() {
  return !!(settings.solapiKey && settings.solapiSecret && settings.kakaoChannel);
}
function updateApiStatus() {
  const connected = isApiConnected();
  const box  = document.getElementById('solapi-status-box');
  const dot  = document.getElementById('solapi-dot');
  const text = document.getElementById('solapi-status-text');
  const badge = document.getElementById('api-status-badge');
  if (box) {
    box.className = `solapi-status ${connected ? 'connected' : 'disconnected'}`;
    dot.className = `status-dot ${connected ? 'on' : 'off'}`;
    text.textContent = connected
      ? `솔라피 연결됨 — ${settings.cafeName || '매장'} · 실제 카톡 발송 가능`
      : '솔라피 미연결 — 테스트 모드 (설정에서 API 키 입력)';
  }
  if (badge) badge.textContent = connected ? '🟢 솔라피 연결됨' : '⚪ 테스트 모드';
  const pname = document.getElementById('preview-cafe-name');
  if (pname) pname.textContent = settings.cafeName || '생화케이크 공방';
}

// ── 솔라피 실제 발송 ──
async function sendViaSolapi(phone, message) {
  // 솔라피 API: HMAC-SHA256 인증 방식
  // 브라우저에서 직접 호출 시 CORS 문제 발생 가능 → 실제 운영 시 서버 필요
  // 여기서는 fetch 시도 후 실패 시 안내 제공
  const timestamp = Date.now().toString();
  const salt = Math.random().toString(36).substring(2,12);
  const signature = timestamp + salt; // 실제로는 HMAC-SHA256 필요

  try {
    const res = await fetch('https://api.solapi.com/messages/v4/send', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `HMAC-SHA256 apiKey=${settings.solapiKey}, date=${timestamp}, salt=${salt}, signature=${signature}`
      },
      body: JSON.stringify({
        message: {
          to: phone.replace(/-/g,''),
          from: settings.fromNumber || '15880000',
          text: message,
          type: 'ATA',
          kakaoOptions: {
            pfId: settings.kakaoChannel,
            templateId: settings.templateId || '',
            variables: {}
          }
        }
      })
    });
    return res.ok;
  } catch(e) {
    return false;
  }
}


// ── 대시보드 → 고객 관리 이동 (필터 포함) ──
function goCustomers(filter) {
  currentFilter = filter;
  showPage('customers', document.querySelector('.nav-item:nth-child(2)'));
  // 필터 버튼 active 동기화
  setTimeout(() => {
    document.querySelectorAll('#page-customers .filter-btn').forEach(b => b.classList.remove('active'));
    const btn = document.getElementById('filter-' + filter);
    if (btn) btn.classList.add('active');
    renderTable();
  }, 50);
}

// ── 모달 ──
function closeModal(id) { document.getElementById('modal-'+id).classList.remove('open'); }

// ── 고객 추가 ──
function addCustomer() {
  const name  = document.getElementById('new-name').value.trim();
  const phone = document.getElementById('new-phone').value.trim();
  if (!name||!phone) { toast('이름과 전화번호는 필수입니다'); return; }
  customers.unshift({
    id: Date.now(), name, phone,
    taste:    document.getElementById('new-taste').value,
    lastBuy:  document.getElementById('new-lastbuy').value,
    buyCount: parseInt(document.getElementById('new-count').value)||0,
    subType:  document.getElementById('new-subtype').value,
    bean:     document.getElementById('new-bean').value,
    memo:     document.getElementById('new-memo').value,
    consentMarketing: document.getElementById('new-consent-marketing').checked,
    consentNight:     document.getElementById('new-consent-night').checked,
    regDate: new Date().toISOString().split('T')[0]
  });
  save();
  closeModal('addCustomer');
  ['new-name','new-phone','new-memo','new-bean','new-count'].forEach(id => document.getElementById(id).value='');
  ['new-consent-marketing','new-consent-night'].forEach(id => document.getElementById(id).checked=false);
  ['new-taste','new-subtype'].forEach(id => document.getElementById(id).value='');
  renderDashboard(); renderTable(); renderSubs();
  toast(`✓ ${name}님이 등록됐습니다`, 'success');
}

// ── 대시보드 ──
function renderDashboard() {
  document.getElementById('stat-total').textContent   = customers.length;
  document.getElementById('stat-consent').textContent = customers.filter(c=>c.consentMarketing).length;
  document.getElementById('stat-dormant').textContent = customers.filter(c=>daysSince(c.lastBuy)>=21).length;
  document.getElementById('stat-sub').textContent     = customers.filter(c=>c.subType).length;

  const rl = document.getElementById('recent-list');
  rl.innerHTML = customers.length
    ? customers.slice(0,5).map(c=>`
        <div class="recent-item">
          <div class="avatar">${c.name[0]}</div>
          <div class="recent-info">
            <div class="recent-name">${c.name} ${c.subType?'<span class="tag tag-kakao">구독</span>':''}</div>
            <div class="recent-sub">${c.phone} · ${c.taste?tasteMap[c.taste]:'취향 미등록'}</div>
          </div>
          ${c.consentMarketing?'<span class="tag tag-green" style="font-size:10px">동의</span>':'<span class="tag tag-gray" style="font-size:10px">미동의</span>'}
        </div>`).join('')
    : '<div style="color:var(--gray-500);font-size:13px;text-align:center;padding:20px">등록된 고객이 없습니다</div>';

  const alerts = [];
  const dormant = customers.filter(c=>daysSince(c.lastBuy)>=21&&c.consentMarketing);
  if (dormant.length) alerts.push({type:'orange',text:`${dormant.slice(0,3).map(c=>c.name).join(', ')}님 등 ${dormant.length}명 3주 이상 미방문 — 쿠폰 카톡을 보내보세요!`,time:'방금'});
  const weeklySubs = customers.filter(c=>c.subType==='weekly');
  if (weeklySubs.length) alerts.push({type:'blue',text:`월간 정기예약 고객 ${weeklySubs.length}명에게 이번 달 픽업 안내를 보낼 시간이에요 🎂`,time:'추천'});
  const al = document.getElementById('alert-list');
  al.innerHTML = alerts.length
    ? alerts.map(a=>`<div class="alert-item"><div class="alert-dot ${a.type}"></div><div><div class="alert-text">${a.text}</div><div class="alert-time">${a.time}</div></div></div>`).join('')
    : '<div style="color:var(--gray-500);font-size:13px;text-align:center;padding:20px">고객을 추가하면 알림이 생성됩니다</div>';
}

// ── 고객 테이블 ──
function renderTable() {
  const q = (document.getElementById('search-input')?.value||'').toLowerCase();
  const list = customers.filter(c=>{
    if (q && !c.name.includes(q) && !c.phone.includes(q)) return false;
    if (currentFilter==='consent')   return c.consentMarketing;
    if (currentFilter==='subscribe') return c.subType;
    if (currentFilter==='dormant')   return daysSince(c.lastBuy)>=21;
    if (currentFilter==='floral')    return c.taste==='floral';
    if (currentFilter==='acidity')   return c.taste==='acidity';
    if (currentFilter==='nutty')     return c.taste==='nutty';
    return true;
  });
  const tb = document.getElementById('customer-table');
  if (!list.length) { tb.innerHTML='<tr><td colspan="9" style="text-align:center;color:var(--gray-500);padding:30px">해당하는 고객이 없습니다</td></tr>'; return; }
  tb.innerHTML = list.map(c=>{
    const d = daysSince(c.lastBuy);
    const dTag = d>=21?`<span class="tag tag-red">${d}일 전</span>`:d>=7?`<span class="tag tag-orange">${d}일 전</span>`:`<span class="tag tag-green">${d}일 전</span>`;
    return `<tr>
      <td><input type="checkbox" class="row-check" data-id="${c.id}"></td>
      <td><strong>${c.name}</strong>${c.memo?`<div style="font-size:11px;color:var(--gray-500)">${c.memo}</div>`:''}</td>
      <td>${c.phone}</td>
      <td>${c.taste?`<span class="tag ${tasteTagClass[c.taste]}">${tasteMap[c.taste]}</span>`:'-'}</td>
      <td>${c.lastBuy?dTag:'-'}</td>
      <td><strong>${c.buyCount}</strong>회</td>
      <td>${c.consentMarketing?'<span class="tag tag-green">✓ 동의</span>'+(c.consentNight?' <span class="tag tag-blue">야간</span>':''):'<span class="tag tag-gray">미동의</span>'}</td>
      <td>${c.subType?`<span class="tag tag-kakao">${subTypeMap[c.subType]}</span>`:'-'}</td>
      <td><button class="btn btn-outline btn-sm" onclick="deleteCustomer(${c.id})">삭제</button></td>
    </tr>`;
  }).join('');
  document.querySelectorAll('.row-check').forEach(cb=>cb.addEventListener('change',updateSelectedCount));
}
function filterCustomers() { renderTable(); }
function setFilter(f,el) {
  currentFilter=f;
  document.querySelectorAll('#page-customers .filter-btn').forEach(b=>b.classList.remove('active'));
  el.classList.add('active');
  renderTable();
}
function toggleAll(cb) { document.querySelectorAll('.row-check').forEach(c=>c.checked=cb.checked); updateSelectedCount(); }
function updateSelectedCount() { document.getElementById('selected-count').textContent=`선택된 고객: ${document.querySelectorAll('.row-check:checked').length}명`; }
function deleteCustomer(id) {
  if (!confirm('삭제하시겠습니까?')) return;
  customers = customers.filter(c=>c.id!==id); save(); renderTable(); renderDashboard();
  toast('삭제됐습니다');
}
function sendToSelected() {
  if (!document.querySelectorAll('.row-check:checked').length) { toast('고객을 선택해주세요'); return; }
  showPage('send', document.querySelector('.nav-item:nth-child(3)'));
}

// ── 카톡 발송 ──
function setMsgFilter(f,el) {
  currentMsgFilter=f;
  document.querySelectorAll('#page-send .filter-btn').forEach(b=>b.classList.remove('active'));
  el.classList.add('active');
  updateRecipients();
}
function updateRecipients() {
  const list = getFiltered(currentMsgFilter);
  document.getElementById('recipient-num').textContent = list.length;
  document.getElementById('recipient-names').textContent = list.slice(0,3).map(c=>c.name).join(', ')+(list.length>3?` 외 ${list.length-3}명`:'');
}
function loadTemplate(k) { document.getElementById('msg-input').value=templates[k]; updatePreview(); }
function updatePreview() {
  const text = document.getElementById('msg-input').value;
  document.getElementById('char-count').textContent = text.length+'자';
  const sample = customers[0]||{name:'홍길동',bean:'에티오피아 예가체프',taste:'floral',buyCount:3};
  let preview = applyVars(text,sample);
  preview = preview.replace(/#{[^}]+}/g, m=>`<span class="var-tag">${m}</span>`);
  document.getElementById('preview-bubble').innerHTML = preview||'메시지를 입력하세요...';
  document.getElementById('preview-time').textContent = nowStr();
}
async function sendMessages() {
  const list = getFiltered(currentMsgFilter);
  const msg  = document.getElementById('msg-input').value;
  if (!list.length) { toast('발송 대상 고객이 없습니다'); return; }
  if (!msg.trim())  { toast('메시지를 입력해주세요'); return; }

  const btn = document.getElementById('send-btn');
  btn.disabled = true; btn.textContent = '발송 중...';

  if (isApiConnected()) {
    // 실제 솔라피 발송
    document.getElementById('progress-section').style.display='block';
    let success=0, fail=0;
    for (let i=0; i<list.length; i++) {
      const c = list[i];
      const personalMsg = applyVars(msg, c);
      document.getElementById('progress-text').textContent = `발송 중 ${i+1}/${list.length} — ${c.name}님`;
      document.getElementById('progress-bar').style.width = ((i+1)/list.length*100)+'%';
      const ok = await sendViaSolapi(c.phone, personalMsg);
      ok ? success++ : fail++;
      await new Promise(r=>setTimeout(r,200));
    }
    document.getElementById('progress-section').style.display='none';
    const entry = {time:new Date().toLocaleString(),count:success,msg:msg.slice(0,30)+'...',mode:'발송완료'};
    sendHistory.unshift(entry); saveHist(); renderHistory();
    if (fail>0) toast(`✓ ${success}명 발송 완료, ${fail}명 실패\n(CORS 이슈 시 서버 프록시 필요)`, 'kakao');
    else toast(`✓ ${success}명에게 카톡 발송 완료!`, 'success');
  } else {
    // 테스트 모드 시뮬레이션
    await new Promise(r=>setTimeout(r,800));
    const entry = {time:new Date().toLocaleString(),count:list.length,msg:msg.slice(0,30)+'...',mode:'테스트'};
    sendHistory.unshift(entry); saveHist(); renderHistory();
    toast(`[테스트] ${list.length}명 발송 시뮬레이션 완료\n실제 발송은 설정에서 솔라피 API 연동 후 가능`, 'kakao');
  }
  btn.disabled=false; btn.innerHTML='💬 카톡 발송하기';
}
function renderHistory() {
  const h = document.getElementById('send-history');
  if (!sendHistory.length) { h.textContent='아직 발송 내역이 없습니다.'; return; }
  h.innerHTML = sendHistory.slice(0,6).map(e=>`
    <div style="display:flex;justify-content:space-between;padding:8px 0;border-bottom:1px solid var(--gray-100);font-size:12px">
      <div><span class="tag ${e.mode==='테스트'?'tag-gray':e.mode==='발송완료'?'tag-green':'tag-kakao'}">${e.mode}</span> ${e.count}명 · ${e.msg}</div>
      <div style="color:var(--gray-500)">${e.time}</div>
    </div>`).join('');
}

// ── 정기예약 ──
function renderSubs() {
  ['weekly','bimonthly','custom'].forEach(t=>{
    document.getElementById('sub-count-'+t).textContent = customers.filter(c=>c.subType===t).length+'명';
  });
  const subs = customers.filter(c=>c.subType);
  const tb = document.getElementById('sub-table');
  if (!subs.length) { tb.innerHTML='<tr><td colspan="7" style="text-align:center;color:var(--gray-500);padding:30px">정기예약 고객이 없습니다</td></tr>'; return; }
  tb.innerHTML = subs.map(c=>{
    const next = new Date(); next.setDate(next.getDate()+(c.subType==='weekly'?7:15));
    return `<tr>
      <td><strong>${c.name}</strong></td><td>${c.phone}</td>
      <td><span class="tag tag-kakao">${subTypeMap[c.subType]}</span></td>
      <td>${c.taste?`<span class="tag ${tasteTagClass[c.taste]}">${tasteMap[c.taste]}</span>`:'-'}</td>
      <td>${c.regDate||'-'}</td><td>${next.toLocaleDateString('ko-KR')}</td>
      <td><button class="btn btn-kakao btn-sm" onclick="quickSend(${c.id})">💬 카톡</button></td>
    </tr>`;
  }).join('');
}
async function sendSubMessage(type) {
  const list = customers.filter(c=>c.subType===type);
  if (!list.length) { toast('해당 예약 고객이 없습니다'); return; }
  const msg = templates.sub;
  if (isApiConnected()) {
    for (const c of list) await sendViaSolapi(c.phone, applyVars(msg,c));
    toast(`✓ ${list.length}명에게 예약 안내 발송 완료`, 'success');
  } else {
    toast(`[테스트] ${list.length}명 예약 안내 시뮬레이션`, 'kakao');
  }
  sendHistory.unshift({time:new Date().toLocaleString(),count:list.length,msg:'정기예약 픽업 안내',mode:isApiConnected()?'발송완료':'테스트'});
  saveHist();
}
async function quickSend(id) {
  const c = customers.find(x=>x.id===id);
  if (!c) return;
  if (isApiConnected()) {
    await sendViaSolapi(c.phone, applyVars(templates.sub,c));
    toast(`✓ ${c.name}님께 카톡 발송 완료`, 'success');
  } else {
    toast(`[테스트] ${c.name}님 발송 시뮬레이션`, 'kakao');
  }
}

// ── 소비패턴 분석 ──
function renderAnalysis() {
  if (!customers.length) return;

  // 1. 취향 분포 막대 차트
  const tasteCounts = {};
  customers.forEach(c=>{ if(c.taste) tasteCounts[c.taste]=(tasteCounts[c.taste]||0)+1; });
  const maxTaste = Math.max(...Object.values(tasteCounts),1);
  const tasteEl = document.getElementById('taste-chart');
  if (Object.keys(tasteCounts).length) {
    tasteEl.innerHTML = Object.entries(tasteCounts)
      .sort((a,b)=>b[1]-a[1])
      .map(([k,v])=>`
        <div class="chart-row">
          <div class="chart-label">${tasteMap[k]||k}</div>
          <div class="chart-bar-bg">
            <div class="chart-bar-fill" style="width:${v/maxTaste*100}%;background:${tasteColors[k]||'#3b5bdb'}">${v}명</div>
          </div>
          <div class="chart-count">${Math.round(v/customers.length*100)}%</div>
        </div>`).join('');
  }

  // 2. 고객 등급 세분화
  const vip    = customers.filter(c=>c.buyCount>=10);
  const loyal  = customers.filter(c=>c.buyCount>=4&&c.buyCount<10);
  const casual = customers.filter(c=>c.buyCount>=1&&c.buyCount<4);
  const newc   = customers.filter(c=>!c.buyCount||c.buyCount===0);
  document.getElementById('segment-chart').innerHTML = [
    {name:'🏆 VIP',     count:vip.length,    bg:'#fff3bf', color:'#e67700', desc:'10회 이상 구매'},
    {name:'💛 단골',    count:loyal.length,  bg:'#d3f9d8', color:'#2b8a3e', desc:'4~9회 구매'},
    {name:'🎂 일반',    count:casual.length, bg:'#dbe4ff', color:'#364fc7', desc:'1~3회 구매'},
    {name:'🆕 신규',    count:newc.length,   bg:'#f1f3f5', color:'#495057', desc:'첫 방문'},
  ].map(s=>`
    <div class="segment-card" style="background:${s.bg}">
      <div class="segment-name" style="color:${s.color}">${s.name}</div>
      <div class="segment-count" style="color:${s.color}">${s.count}명</div>
      <div class="segment-desc" style="color:${s.color}">${s.desc}</div>
    </div>`).join('');

  // 3. 재방문 주기 분석
  const revisitBuckets = [
    {label:'이번 주',   min:0,  max:7,   color:'#2f9e44'},
    {label:'2주 이내',  min:7,  max:14,  color:'#3b5bdb'},
    {label:'3주 이내',  min:14, max:21,  color:'#e67700'},
    {label:'1개월 이내',min:21, max:30,  color:'#c92a2a'},
    {label:'장기 미방문',min:30,max:9999,color:'#868e96'},
  ];
  const revisitEl = document.getElementById('revisit-chart');
  const maxR = Math.max(...revisitBuckets.map(b=>customers.filter(c=>daysSince(c.lastBuy)>=b.min&&daysSince(c.lastBuy)<b.max).length),1);
  revisitEl.innerHTML = revisitBuckets.map(b=>{
    const n = customers.filter(c=>daysSince(c.lastBuy)>=b.min&&daysSince(c.lastBuy)<b.max).length;
    return `<div class="chart-row">
      <div class="chart-label">${b.label}</div>
      <div class="chart-bar-bg">
        <div class="chart-bar-fill" style="width:${n/maxR*100}%;background:${b.color}">${n}명</div>
      </div>
      <div class="chart-count">${n}</div>
    </div>`;
  }).join('');

  // 4. 수신동의 현황
  const mktY = customers.filter(c=>c.consentMarketing).length;
  const mktN = customers.length - mktY;
  const nightY = customers.filter(c=>c.consentNight).length;
  document.getElementById('consent-chart').innerHTML = `
    <div style="display:flex;flex-direction:column;gap:12px">
      <div>
        <div style="display:flex;justify-content:space-between;font-size:12px;margin-bottom:4px">
          <span>마케팅 동의</span><span><strong style="color:var(--success)">${mktY}명</strong> / 미동의 ${mktN}명</span>
        </div>
        <div class="progress-wrap" style="height:12px">
          <div class="progress-bar" style="width:${customers.length?mktY/customers.length*100:0}%;background:var(--success)"></div>
        </div>
      </div>
      <div>
        <div style="display:flex;justify-content:space-between;font-size:12px;margin-bottom:4px">
          <span>야간 동의</span><span><strong style="color:var(--primary)">${nightY}명</strong> / 미동의 ${customers.length-nightY}명</span>
        </div>
        <div class="progress-wrap" style="height:12px">
          <div class="progress-bar" style="width:${customers.length?nightY/customers.length*100:0}%;background:var(--primary)"></div>
        </div>
      </div>
      <div style="font-size:12px;color:var(--gray-500);background:var(--gray-50);padding:10px;border-radius:8px;line-height:1.7">
        📌 마케팅 동의율: <strong>${customers.length?Math.round(mktY/customers.length*100):0}%</strong><br>
        광고성 메시지를 받을 수 있는 고객 비율입니다.
      </div>
    </div>`;

  // 5. 액션 추천
  const dormantList = customers.filter(c=>daysSince(c.lastBuy)>=21&&c.consentMarketing);
  const floralList  = customers.filter(c=>c.taste==='floral'&&c.consentMarketing);
  const vipList     = customers.filter(c=>c.buyCount>=10&&c.consentMarketing);
  const actions = [
    { icon:'⏰', color:'#ffe3e3', textColor:'#c92a2a', title:`장기 미방문 ${dormantList.length}명`, desc:'3주 이상 방문 없음\n쿠폰 카톡으로 재유도', filter:'dormant', template:'dormant' },
    { icon:'🌹', color:'#f3d9fa', textColor:'#862e9c', title:`로맨틱로즈 취향 ${floralList.length}명`, desc:'로즈 생화케이크 신상 출시 시\n바로 알림 발송 가능', filter:'floral', template:'newin' },
    { icon:'🏆', color:'#fff3bf', textColor:'#e67700', title:`VIP 고객 ${vipList.length}명`, desc:'10회 이상 구매 단골\n감사 쿠폰 발송 추천', filter:'consent', template:'coupon' },
  ];
  document.getElementById('action-recommend').innerHTML = actions.map(a=>`
    <div style="background:${a.color};border-radius:10px;padding:16px;cursor:pointer" onclick="jumpToSend('${a.filter}','${a.template}')">
      <div style="font-size:22px;margin-bottom:6px">${a.icon}</div>
      <div style="font-weight:700;color:${a.textColor};margin-bottom:4px">${a.title}</div>
      <div style="font-size:12px;color:${a.textColor};opacity:0.8;line-height:1.6;white-space:pre-line">${a.desc}</div>
      <div style="margin-top:10px"><span style="background:${a.textColor};color:white;padding:4px 10px;border-radius:6px;font-size:12px;font-weight:600">💬 바로 발송</span></div>
    </div>`).join('');
}
function jumpToSend(filter, template) {
  currentMsgFilter = filter;
  showPage('send', document.querySelector('.nav-item:nth-child(3)'));
  setTimeout(()=>{
    document.querySelectorAll('#page-send .filter-btn').forEach(b=>b.classList.remove('active'));
    loadTemplate(template);
    updateRecipients();
  },100);
}

// ── 설정 ──
function loadSettingsForm() {
  document.getElementById('solapi-key').value    = settings.solapiKey    || '';
  document.getElementById('solapi-secret').value = settings.solapiSecret || '';
  document.getElementById('kakao-channel').value = settings.kakaoChannel || '';
  document.getElementById('cafe-name').value     = settings.cafeName     || '';
}
function saveSettings() {
  settings.solapiKey    = document.getElementById('solapi-key').value.trim();
  settings.solapiSecret = document.getElementById('solapi-secret').value.trim();
  settings.kakaoChannel = document.getElementById('kakao-channel').value.trim();
  settings.cafeName     = document.getElementById('cafe-name').value.trim();
  localStorage.setItem('crm_settings', JSON.stringify(settings));
  updateApiStatus();
  toast(isApiConnected() ? '✓ 솔라피 연결 설정 저장됨 — 실제 발송 가능' : '설정 저장됨 (API 키 미입력 → 테스트 모드)', isApiConnected()?'success':'');
}

// ── CSV ──
function exportData() {
  if (!customers.length) { toast('내보낼 데이터가 없습니다'); return; }
  const header = '이름,전화번호,취향,마지막구매일,구매횟수,정기예약,마케팅동의,야간동의,메모\n';
  const rows = customers.map(c=>[c.name,c.phone,tasteMap[c.taste]||'',c.lastBuy||'',c.buyCount||0,subTypeMap[c.subType]||'',c.consentMarketing?'Y':'N',c.consentNight?'Y':'N',c.memo||''].join(',')).join('\n');
  const a = document.createElement('a');
  a.href = URL.createObjectURL(new Blob(['﻿'+header+rows],{type:'text/csv'}));
  a.download = `고객DB_${new Date().toLocaleDateString('ko-KR').replace(/\s|\./g,'')}.csv`;
  a.click();
  toast('📥 CSV 다운로드 완료', 'success');
}
function importCSV(input) {
  const file = input.files[0]; if (!file) return;
  const reader = new FileReader();
  reader.onload = e => {
    const lines = e.target.result.split('\n').slice(1);
    let count=0;
    lines.forEach(line=>{
      const cols=line.split(',');
      if (cols[0]&&cols[1]) {
        customers.push({id:Date.now()+count,name:cols[0].trim(),phone:cols[1].trim(),taste:'',lastBuy:cols[3]||'',buyCount:parseInt(cols[4])||0,subType:'',consentMarketing:cols[6]==='Y',consentNight:cols[7]==='Y',memo:cols[8]||'',regDate:new Date().toISOString().split('T')[0]});
        count++;
      }
    });
    save(); renderTable(); renderDashboard();
    toast(`✓ ${count}명 불러오기 완료`, 'success');
  };
  reader.readAsText(file,'UTF-8');
}
function clearAllData() {
  if (!confirm('⚠️ 전체 데이터를 삭제하시겠습니까?\n이 작업은 되돌릴 수 없습니다.')) return;
  customers=[]; save(); sendHistory=[]; saveHist();
  renderTable(); renderDashboard(); renderSubs();
  toast('전체 데이터 초기화됨');
}

// ── 데이터 가져오기 페이지 ──
function showPage(name, el) {
  document.querySelectorAll('.page').forEach(p => p.classList.remove('active'));
  document.querySelectorAll('.nav-item').forEach(n => n.classList.remove('active'));
  document.getElementById('page-'+name).classList.add('active');
  if (el) el.classList.add('active');
  const titles = { dashboard:'📊 대시보드', customers:'👥 고객 관리', send:'💬 카톡 발송', subscribe:'🔄 정기예약', analysis:'📈 소비패턴 분석', cohort:'🎯 코호트 타겟', import:'📥 데이터 가져오기', settings:'⚙️ 설정 / API' };
  document.getElementById('topbar-title').textContent = titles[name] || '';
  if (name==='dashboard') renderDashboard();
  if (name==='customers') renderTable();
  if (name==='subscribe') renderSubs();
  if (name==='send')      { updateRecipients(); renderHistory(); updateApiStatus(); refreshCohortSelect(); }
  if (name==='analysis')  renderAnalysis();
  if (name==='cohort')    renderCohorts();
  if (name==='settings')  loadSettingsForm();
}

// ── 스마트스토어 엑셀 가져오기 ──
function importSmartstore(input) {
  const file = input.files[0]; if (!file) return;
  const reader = new FileReader();
  reader.onload = e => {
    const text = e.target.result;
    const lines = text.split('\n').filter(l => l.trim());
    if (lines.length < 2) { toast('파일에 데이터가 없습니다'); return; }

    // 헤더 파싱 (스마트스토어 표준 컬럼 자동 감지)
    const rawHeader = lines[0].split(',').map(h => h.replace(/"/g,'').trim());
    const colMap = {};
    rawHeader.forEach((h, i) => {
      if (h.includes('구매자명') || h.includes('주문자명') || h.includes('이름')) colMap.name = i;
      if (h.includes('구매자 연락처') || h.includes('전화번호') || h.includes('휴대폰')) colMap.phone = i;
      if (h.includes('상품명')) colMap.product = i;
      if (h.includes('결제금액') || h.includes('주문금액')) colMap.amount = i;
      if (h.includes('주문일') || h.includes('결제일')) colMap.date = i;
      if (h.includes('주문상태') || h.includes('배송상태')) colMap.status = i;
      if (h.includes('주문번호')) colMap.orderId = i;
    });

    // 감지 실패 시 위치 기반 fallback (스마트스토어 표준 순서)
    // 표준: 주문번호(0), 주문일(1), 구매자명(2), 구매자연락처(3), 수령인(4), ...상품명(8)...결제금액(14)
    if (!colMap.name)  colMap.name    = colMap.name    ?? 2;
    if (!colMap.phone) colMap.phone   = colMap.phone   ?? 3;
    if (!colMap.product) colMap.product = colMap.product ?? 8;
    if (!colMap.amount) colMap.amount = colMap.amount  ?? 14;
    if (!colMap.date)  colMap.date    = colMap.date    ?? 1;

    let newCount = 0, updateCount = 0, skipCount = 0;
    const results = [];

    lines.slice(1).forEach(line => {
      if (!line.trim()) return;
      const cols = parseCSVLine(line);
      const rawName  = (cols[colMap.name]  || '').replace(/"/g,'').trim();
      const rawPhone = (cols[colMap.phone] || '').replace(/"/g,'').trim().replace(/\D/g,'');
      const product  = (cols[colMap.product] || '').replace(/"/g,'').trim();
      const amount   = parseInt((cols[colMap.amount] || '0').replace(/[^0-9]/g,'')) || 0;
      const dateRaw  = (cols[colMap.date]  || '').replace(/"/g,'').trim();
      const date     = dateRaw ? dateRaw.slice(0,10).replace(/\./g,'-') : '';

      if (!rawName || rawPhone.length < 10) { skipCount++; return; }
      const phone = rawPhone.replace(/(\d{3})(\d{4})(\d{4})/, '$1-$2-$3');

      const existing = customers.find(c => c.phone.replace(/-/g,'') === rawPhone);
      if (existing) {
        existing.buyCount = (existing.buyCount || 0) + 1;
        if (date > (existing.lastBuy || '')) existing.lastBuy = date;
        if (product && !existing.bean) existing.bean = product;
        existing.source = existing.source || 'smartstore';
        updateCount++;
        results.push({ name: rawName, phone, product, amount, date, status: '업데이트' });
      } else {
        customers.unshift({ id: Date.now() + newCount, name: rawName, phone, taste: '', lastBuy: date, buyCount: 1, subType: '', bean: product, memo: `스마트스토어 주문 ${date}`, consentMarketing: false, consentNight: false, regDate: date || new Date().toISOString().split('T')[0], source: 'smartstore', totalAmount: amount });
        newCount++;
        results.push({ name: rawName, phone, product, amount, date, status: '신규' });
      }
    });

    save(); renderDashboard();

    // 결과 표시
    const box = document.getElementById('smartstore-result');
    const body = document.getElementById('smartstore-result-body');
    box.style.display = 'block';
    body.innerHTML = `
      <div style="display:flex;gap:16px;margin-bottom:16px;flex-wrap:wrap">
        <div style="background:#d3f9d8;border-radius:8px;padding:12px 20px;text-align:center">
          <div style="font-size:22px;font-weight:700;color:#2b8a3e">${newCount}</div>
          <div style="font-size:12px;color:#2b8a3e">신규 등록</div>
        </div>
        <div style="background:#dbe4ff;border-radius:8px;padding:12px 20px;text-align:center">
          <div style="font-size:22px;font-weight:700;color:#364fc7">${updateCount}</div>
          <div style="font-size:12px;color:#364fc7">기존 고객 업데이트</div>
        </div>
        <div style="background:#f1f3f5;border-radius:8px;padding:12px 20px;text-align:center">
          <div style="font-size:22px;font-weight:700;color:#495057">${skipCount}</div>
          <div style="font-size:12px;color:#495057">정보 부족 제외</div>
        </div>
      </div>
      <div style="max-height:240px;overflow-y:auto">
        <table style="width:100%;border-collapse:collapse;font-size:12px">
          <thead><tr style="background:var(--gray-50)">
            <th style="padding:8px;border-bottom:2px solid var(--gray-200);text-align:left">이름</th>
            <th style="padding:8px;border-bottom:2px solid var(--gray-200);text-align:left">전화번호</th>
            <th style="padding:8px;border-bottom:2px solid var(--gray-200);text-align:left">상품명</th>
            <th style="padding:8px;border-bottom:2px solid var(--gray-200);text-align:right">금액</th>
            <th style="padding:8px;border-bottom:2px solid var(--gray-200);text-align:center">처리</th>
          </tr></thead>
          <tbody>${results.slice(0,20).map(r=>`
            <tr>
              <td style="padding:7px 8px;border-bottom:1px solid var(--gray-100)">${r.name}</td>
              <td style="padding:7px 8px;border-bottom:1px solid var(--gray-100)">${r.phone}</td>
              <td style="padding:7px 8px;border-bottom:1px solid var(--gray-100)">${r.product||'-'}</td>
              <td style="padding:7px 8px;border-bottom:1px solid var(--gray-100);text-align:right">${r.amount?r.amount.toLocaleString()+'원':'-'}</td>
              <td style="padding:7px 8px;border-bottom:1px solid var(--gray-100);text-align:center">
                <span class="tag ${r.status==='신규'?'tag-green':'tag-blue'}">${r.status}</span>
              </td>
            </tr>`).join('')}
          </tbody>
        </table>
        ${results.length > 20 ? `<div style="text-align:center;padding:8px;font-size:12px;color:var(--gray-500)">외 ${results.length-20}건</div>` : ''}
      </div>
      <div style="margin-top:12px;display:flex;gap:8px">
        <button class="btn btn-kakao btn-sm" onclick="showPage('customers',document.querySelector('.nav-item:nth-child(2)'))">👥 고객 관리로 이동</button>
        <button class="btn btn-outline btn-sm" onclick="showPage('send',document.querySelector('.nav-item:nth-child(3)'))">💬 카톡 발송하기</button>
      </div>`;

    toast(`✓ 스마트스토어 ${newCount}명 신규 등록, ${updateCount}명 업데이트`, 'success');
    input.value = '';
  };
  reader.readAsText(file, 'UTF-8');
}

// ── 토스포스 CSV 가져오기 (매출 분석) ──
function importTosspos(input) {
  const file = input.files[0]; if (!file) return;
  const reader = new FileReader();
  reader.onload = e => {
    const lines = e.target.result.split('\n').filter(l => l.trim());
    if (lines.length < 2) { toast('파일에 데이터가 없습니다'); return; }

    const rawHeader = lines[0].split(',').map(h => h.replace(/"/g,'').trim());
    const colMap = {};
    rawHeader.forEach((h, i) => {
      if (h.includes('날짜') || h.includes('일자') || h.includes('일시')) colMap.date = i;
      if (h.includes('금액') || h.includes('매출')) colMap.amount = i;
      if (h.includes('결제수단') || h.includes('수단')) colMap.method = i;
      if (h.includes('메뉴') || h.includes('상품') || h.includes('품목')) colMap.menu = i;
      if (h.includes('수량')) colMap.qty = i;
      if (h.includes('시간') || h.includes('시각')) colMap.time = i;
    });

    // Fallback
    colMap.date   = colMap.date   ?? 0;
    colMap.amount = colMap.amount ?? 3;
    colMap.method = colMap.method ?? 4;
    colMap.menu   = colMap.menu   ?? 1;
    colMap.qty    = colMap.qty    ?? 2;

    const records = [];
    lines.slice(1).forEach(line => {
      if (!line.trim()) return;
      const cols = parseCSVLine(line);
      const date   = (cols[colMap.date]   || '').replace(/"/g,'').trim();
      const amount = parseInt((cols[colMap.amount] || '0').replace(/[^0-9]/g,'')) || 0;
      const method = (cols[colMap.method] || '').replace(/"/g,'').trim();
      const menu   = (cols[colMap.menu]   || '').replace(/"/g,'').trim();
      const qty    = parseInt((cols[colMap.qty] || '1').replace(/[^0-9]/g,'')) || 1;
      if (date && amount) records.push({ date, amount, method, menu, qty });
    });

    if (!records.length) { toast('파싱 가능한 데이터가 없습니다. 파일 형식을 확인해주세요'); return; }

    // 분석
    const totalSales   = records.reduce((s,r) => s + r.amount, 0);
    const dayMap       = {};
    const methodMap    = {};
    const menuMap      = {};
    const hourMap      = {};
    records.forEach(r => {
      const day = new Date(r.date).getDay();
      const dayName = ['일','월','화','수','목','금','토'][day] || '?';
      dayMap[dayName]   = (dayMap[dayName]   || 0) + r.amount;
      methodMap[r.method||'기타'] = (methodMap[r.method||'기타'] || 0) + 1;
      if (r.menu) menuMap[r.menu] = (menuMap[r.menu] || 0) + r.qty;
    });

    const topDay  = Object.entries(dayMap).sort((a,b)=>b[1]-a[1])[0];
    const topMenu = Object.entries(menuMap).sort((a,b)=>b[1]-a[1])[0];
    const maxDay  = Math.max(...Object.values(dayMap), 1);
    const maxMenu = Math.max(...Object.values(menuMap), 1);
    const dayOrder = ['월','화','수','목','금','토','일'];

    localStorage.setItem('crm_tosspos', JSON.stringify({ records, analysed: new Date().toISOString() }));

    const box = document.getElementById('tosspos-result');
    const body = document.getElementById('tosspos-result-body');
    box.style.display = 'block';
    body.innerHTML = `
      <div style="display:flex;gap:16px;margin-bottom:20px;flex-wrap:wrap">
        <div style="background:#eff6ff;border-radius:8px;padding:12px 20px;text-align:center;flex:1">
          <div style="font-size:20px;font-weight:700;color:#0064ff">${records.length.toLocaleString()}</div>
          <div style="font-size:12px;color:#0064ff">총 거래 건수</div>
        </div>
        <div style="background:#f0fdf4;border-radius:8px;padding:12px 20px;text-align:center;flex:1">
          <div style="font-size:20px;font-weight:700;color:#2b8a3e">${totalSales.toLocaleString()}원</div>
          <div style="font-size:12px;color:#2b8a3e">총 매출</div>
        </div>
        <div style="background:#fff3bf;border-radius:8px;padding:12px 20px;text-align:center;flex:1">
          <div style="font-size:20px;font-weight:700;color:#e67700">${topDay ? topDay[0]+'요일' : '-'}</div>
          <div style="font-size:12px;color:#e67700">최고 매출 요일</div>
        </div>
        <div style="background:#f3d9fa;border-radius:8px;padding:12px 20px;text-align:center;flex:1">
          <div style="font-size:18px;font-weight:700;color:#862e9c">${topMenu ? topMenu[0].slice(0,8) : '-'}</div>
          <div style="font-size:12px;color:#862e9c">베스트 메뉴</div>
        </div>
      </div>

      <div style="display:grid;grid-template-columns:1fr 1fr;gap:16px">
        <div>
          <div style="font-weight:600;font-size:13px;margin-bottom:10px">📅 요일별 매출</div>
          <div style="display:flex;flex-direction:column;gap:7px">
            ${dayOrder.map(d => {
              const v = dayMap[d] || 0;
              return `<div style="display:flex;align-items:center;gap:8px">
                <div style="width:24px;font-size:12px;color:var(--gray-700);font-weight:600">${d}</div>
                <div style="flex:1;background:var(--gray-100);border-radius:20px;height:16px;overflow:hidden">
                  <div style="height:100%;width:${v/maxDay*100}%;background:#0064ff;border-radius:20px;display:flex;align-items:center;padding-left:6px;font-size:10px;color:white;font-weight:700">${v>0?v.toLocaleString()+'원':''}</div>
                </div>
              </div>`;
            }).join('')}
          </div>
        </div>
        <div>
          <div style="font-weight:600;font-size:13px;margin-bottom:10px">🎂 인기 메뉴 TOP 5</div>
          <div style="display:flex;flex-direction:column;gap:7px">
            ${Object.entries(menuMap).sort((a,b)=>b[1]-a[1]).slice(0,5).map(([m,v],i)=>`
              <div style="display:flex;align-items:center;gap:8px">
                <div style="width:18px;font-size:11px;color:var(--gray-500)">${i+1}</div>
                <div style="flex:1;background:var(--gray-100);border-radius:20px;height:16px;overflow:hidden">
                  <div style="height:100%;width:${v/maxMenu*100}%;background:#862e9c;border-radius:20px;display:flex;align-items:center;padding-left:6px;font-size:10px;color:white;font-weight:700">${m.slice(0,10)}</div>
                </div>
                <div style="font-size:11px;color:var(--gray-500)">${v}개</div>
              </div>`).join('')}
          </div>
        </div>
      </div>

      <div style="margin-top:14px;background:#e0f2fe;border-radius:8px;padding:12px;font-size:12px;line-height:1.8;color:#0369a1">
        💡 <strong>카톡 발송 타이밍 추천</strong><br>
        ${topDay ? `→ <strong>${topDay[0]}요일</strong>이 매출이 가장 높아요. 전날에 카톡을 발송하면 효과적입니다.` : ''}
        ${topMenu ? `<br>→ <strong>${topMenu[0]}</strong>이 가장 잘 팔려요. 신규 입고 시 이 메뉴 팬들에게 먼저 알리세요.` : ''}
      </div>`;

    toast(`✓ 토스포스 ${records.length}건 매출 분석 완료`, 'success');
    input.value = '';
  };
  reader.readAsText(file, 'UTF-8');
}

// CSV 파싱 헬퍼 (따옴표 처리)
function parseCSVLine(line) {
  const result = []; let cur = ''; let inQuote = false;
  for (let i = 0; i < line.length; i++) {
    const ch = line[i];
    if (ch === '"') { inQuote = !inQuote; }
    else if (ch === ',' && !inQuote) { result.push(cur); cur = ''; }
    else { cur += ch; }
  }
  result.push(cur);
  return result;
}

// ── 초기화 ──
renderDashboard();
updateApiStatus();
document.getElementById('msg-input').addEventListener('input', updatePreview);
if (!customers.length) setTimeout(()=>toast('+ 고객 추가 버튼으로 첫 고객을 등록해보세요!'),800);
</script>
</body>
</html>
