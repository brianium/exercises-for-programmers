(ns brianium.retirement.input.spec
  (:require [clojure.spec.alpha :as s]))

(defn valid-state?
  [{:keys [state value]}]
  (case state
    :success (not (nil? value))
    :negative (and (not (nil? value)) (neg? value))
    true))

(s/def :parse/state #{:success :failure :empty :negative})

(s/def :parse/value (s/nilable int?))

(s/def ::state (s/and (s/keys :req-un [:parse/state]
                              :opt-un [:parse/value])
                      valid-state?))
