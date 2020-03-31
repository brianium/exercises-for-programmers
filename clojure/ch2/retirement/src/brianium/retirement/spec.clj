(ns brianium.retirement.spec
  (:require [clojure.spec.alpha :as s])
  (:import (java.util Date)))

(s/def ::age int?)

(s/def ::retirement-age int?)

(s/def ::retiree (s/keys :reg-un [::age ::retirement-age]))

(s/def ::year int?)

(s/def ::date #(instance? Date %))
