const apiUrl = "https://localhost:7123/api/Patients"
const patientList = document.getElementById("patientList");


document.addEventListener("DOMContentLoaded", function () {
    HastaListele();
});

function HastaListele() {
    patientList.classList.remove("d-none");

    fetch(apiUrl)
        .then((res) => res.json())
        .then((data) => {
            patientList.innerHTML = "";

            data.forEach(function (p) {
                patientList.innerHTML += `

                                             <div class="col-12 col-md-6 col-lg-3  ">
                                                <div class="card h-100 bg-dark text-white border-secondary shadow" style="border-top: 5px solid #0d6efd">
                                                    <div class="card-body">
                                                        <div class="d-flex justify-content-between align-items-center mb-3">
                                                            <h4 class="card-title mb-0">${p.animalName}</h4>
                                                            <span class="badge rounded-pill" style="background-color: #854d0e; color: #fbbf24">${p.species}</span>
                                                        </div>
                                                        <div class="card-text mb-4">
                                                            <p class="mb-1 text-secondary">
                                                                <strong>Cins:</strong> <span class="text-light">${p.breed}</span>
                                                            </p>
                                                            <p class="mb-1 text-secondary">
                                                                <strong>Sahip:</strong> <span class="text-light">${p.ownerName}</span>
                                                            </p>
                                                        </div>
                                                        <div class="d-flex gap-2">
                                                            <button class="btn btn-primary w-100" data-id="${p.id}" onclick="duzenle(${p.id})" data-bs-target="#designModal"data-bs-toggle="modal">Düzenle</button>
                                                            <button class="btn btn-danger w-100"onclick="hastaSil(${p.id})">Sil</button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                                `;


            });
        });
}

const animalNameInput = document.getElementById("patient-name")
const treatmentInput = document.getElementById("treatment-text");
let seciliHastaId = null;

function duzenle(id) {
    console.log("Tıklanan hasta ID:", id);
    seciliHastaId = id;

    fetch(apiUrl + "/" + id)
        .then(res => res.json())
        .then(data => {
            animalNameInput.value = data.animalName;
            treatmentInput.value = data.treatmentDescription;
        });
}


document.getElementById("btnKaydet").addEventListener("click", function () {
    hastaGuncelle();
})

function hastaGuncelle() {

    const guncelHasta = {
        animalName: animalNameInput.value,
        treatmentDescription: treatmentInput.value
    };

    fetch(apiUrl + "/" + seciliHastaId, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(guncelHasta)
    })
        .then(res => {
            if (res.ok) {
                alert("Güncelleme başarılı");

                const modal = bootstrap.Modal.getInstance(
                    document.getElementById("designModal")
                );
                modal.hide();

                HastaListele();
            };

        });

}


// post
const addFormContainer = document.getElementById("addFormContainer");
const hastaForm = document.getElementById("hastaForm");

function yeniKayitAc() {
    hastaForm.reset();
    addFormContainer.classList.remove("d-none");

    addFormContainer.scrollIntoView({
        behavior: "smooth"
    });
};



hastaForm.addEventListener("submit", function (e) {
    e.preventDefault();

    const hasta = {
        animalName: hastaForm.AnimalName.value,
        treatmentDescription: hastaForm.TreatmentDescription.value,
        species: hastaForm.Species.value,
        breed: hastaForm.Breed.value,
        ownerName: hastaForm.OwnerName.value,
        visitDate: hastaForm.VisitDate.value,
        isVaccinated: hastaForm.IsVaccinated.checked
    };

    fetch("https://localhost:7123/api/Patients", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(hasta)
    })
        .then(res => {
            if (!res.ok) throw new Error();
        })
        .then(() => {
            hastaForm.reset();
            addFormContainer.classList.add("d-none");
            HastaListele();
        });
});
function formuKapat() {
    addFormContainer.classList.add("d-none");
    hastaForm.reset();
}

//Delete
function hastaSil(id) {
    if (!confirm("Bu hastayı silmek istediğine emin misin?")) return;

    fetch("https://localhost:7123/api/Patients/" + id, {
        method: "DELETE"
    })
        .then(res => {
            if (!res.ok) throw new Error();
        })
        .then(() => {
            HastaListele();
        });
}