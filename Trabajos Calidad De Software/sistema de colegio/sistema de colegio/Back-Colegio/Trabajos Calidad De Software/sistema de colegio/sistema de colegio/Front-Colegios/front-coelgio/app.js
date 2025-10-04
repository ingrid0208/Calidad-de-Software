const apiBase = "http://localhost:5165/api"; // tu backend

document.getElementById("form-colegio").addEventListener("submit", async (e) => {
  e.preventDefault();
  const data = {
    name: document.getElementById("colegio-nombre").value,
    address: document.getElementById("colegio-direccion").value,
    phone: document.getElementById("colegio-contacto").value,
    username: document.getElementById("colegio-usuario").value,
    password: document.getElementById("colegio-clave").value,
  };

  const res = await fetch(`${apiBase}/Colegio`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });

  if (res.ok) {
    alert("Colegio creado!");
    loadColegios();
  } else {
    alert("Error al crear colegio");
  }
});

// ================== CREAR CURSO ==================
document.getElementById("form-curso").addEventListener("submit", async (e) => {
  e.preventDefault();
  const data = {
    name: document.getElementById("curso-nombre").value,
    description: document.getElementById("curso-descripcion").value,
    colegioId: parseInt(document.getElementById("curso-colegioId").value),
  };

  const res = await fetch(`${apiBase}/Cursos`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });

  if (res.ok) {
    alert("Curso creado!");
    loadCursos();
  } else {
    alert("Error al crear curso");
  }
});

// ================== CREAR SALÓN ==================
document.getElementById("form-salon").addEventListener("submit", async (e) => {
  e.preventDefault();
  const data = {
    name: document.getElementById("salon-nombre").value,
    cursoId: parseInt(document.getElementById("salon-cursoId").value),
  };

  const res = await fetch(`${apiBase}/Salones`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });

  if (res.ok) {
    alert("Salón creado!");
    loadSalones();
  } else {
    alert("Error al crear salón");
  }
});

// ================== DISTRIBUIR ==================
document.getElementById("form-distribucion").addEventListener("submit", async (e) => {
  e.preventDefault();
  const colegioId = parseInt(document.getElementById("distribucion-colegioId").value);
  const totalEstudiantes = parseInt(document.getElementById("distribucion-estudiantes").value);

  const res = await fetch(`${apiBase}/Distribucion/${colegioId}/distribuir/${totalEstudiantes}`);
  if (!res.ok) {
    alert("Error en distribución");
    return;
  }

  const data = await res.json();
  const tbody = document.querySelector("#tabla-distribucion tbody");
  tbody.innerHTML = "";

  data.forEach(d => {
    const tr = document.createElement("tr");
    tr.innerHTML = `
      <td>${d.colegioName ?? "N/A"}</td>
      <td>${d.cursoName}</td>
      <td>${d.salonName}</td>
      <td>${d.cantidadAsignada}</td>
    `;
    tbody.appendChild(tr);
  });

  document.getElementById("tabla-distribucion").style.display = "table";
});

// ================== LISTADOS ==================
async function loadColegios() {
  const res = await fetch(`${apiBase}/Colegio`);
  const data = await res.json();

  // Tabla colegios
  const tbody = document.querySelector("#tabla-colegios tbody");
  tbody.innerHTML = "";

  // Selects dependientes
  const selectCurso = document.getElementById("curso-colegioId");
  const selectDistribucion = document.getElementById("distribucion-colegioId");
  selectCurso.innerHTML = '<option value="">Seleccione un colegio</option>';
  selectDistribucion.innerHTML = '<option value="">Seleccione un colegio</option>';

  data.forEach(c => {
    const tr = document.createElement("tr");
    tr.innerHTML = `
      <td>${c.id}</td>
      <td>${c.name}</td>
      <td>${c.address}</td>
      <td>${c.phone}</td>
    `;
    tbody.appendChild(tr);

    // Opciones para selects
    const optCurso = document.createElement("option");
    optCurso.value = c.id;
    optCurso.textContent = `${c.name} (ID:${c.id})`;
    selectCurso.appendChild(optCurso);

    const optDist = document.createElement("option");
    optDist.value = c.id;
    optDist.textContent = `${c.name} (ID:${c.id})`;
    selectDistribucion.appendChild(optDist);
  });
}

async function loadCursos() {
  const res = await fetch(`${apiBase}/Cursos`);
  const data = await res.json();

  const tbody = document.querySelector("#tabla-cursos tbody");
  tbody.innerHTML = "";

  const selectSalon = document.getElementById("salon-cursoId");
  selectSalon.innerHTML = '<option value="">Seleccione un curso</option>';

  data.forEach(c => {
    const tr = document.createElement("tr");
    tr.innerHTML = `
      <td>${c.id}</td>
      <td>${c.name}</td>
      <td>${c.description}</td>
      <td>${c.colegioId}</td>
    `;
    tbody.appendChild(tr);

    const optSalon = document.createElement("option");
    optSalon.value = c.id;
    optSalon.textContent = `${c.name} (ColegioId:${c.colegioId})`;
    selectSalon.appendChild(optSalon);
  });
}

async function loadSalones() {
  const res = await fetch(`${apiBase}/Salones`);
  const data = await res.json();

  const tbody = document.querySelector("#tabla-salones tbody");
  tbody.innerHTML = "";

  data.forEach(s => {
    const tr = document.createElement("tr");
    tr.innerHTML = `
      <td>${s.id}</td>
      <td>${s.name}</td>
      <td>${s.cursoId}</td>
      <td>${s.amount}</td>
    `;
    tbody.appendChild(tr);
  });
}

// Inicializar
loadColegios();
loadCursos();
loadSalones();
